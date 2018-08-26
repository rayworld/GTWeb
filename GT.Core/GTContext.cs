using System.Data.Entity;

namespace GT.Core
{
    public class GTContext : DbContext
    {

        /// <summary>
        /// 管理员集合
        /// </summary>
        public DbSet<Administrator> Administrators { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<User> Users { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public GTContext() : base("GTConnection")
        {
            Database.SetInitializer<GTContext>(new CreateDatabaseIfNotExists<GTContext>());
        }
    }
}