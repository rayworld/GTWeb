/*==============================
版本：v0.1
创建：2016.3.15
作者：洞庭夕照
博客:http://mzwhj.cnblogs.com
==============================*/

using GT.Utility;
using LinqKit;
using System.Collections.Generic;
using System.Linq;

namespace GT.Core
{
    /// <summary>
    /// 用户管理类
    /// </summary>
    public class UserManager : BaseManager<User>
    {
        /// <summary>
        /// 添加【返回值Response.Code:0-失败，1-成功，2-账号已存在，3-Email已存在】
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public override Response Add(User user)
        {
            Response _resp = new Response();
            //账号是否存在
            if (!string.IsNullOrEmpty(user.Username) && HasUsername(user.Username))
            {
                _resp.Code = 2;
                _resp.Message = "用户名已存在";
            }
            //Email是否存在
            if (!string.IsNullOrEmpty(user.Email) && HasUsername(user.Email))
            {
                _resp.Code = 3;
                _resp.Message = "Email已存在";
            }
            if (_resp.Code == 0) _resp = base.Add(user);
            return _resp;
        }
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pagingUser">分页数据</param>
        /// <param name="roleID">角色ID</param>
        /// <param name="username">用户名</param>
        /// <param name="name">名称</param>
        /// <param name="sex">性别</param>
        /// <param name="email">Email</param>
        /// <param name="order">排序【null（默认）-ID降序，0-ID升序，1-ID降序，2-注册时间降序，3-注册时间升序，4-最后登录时间升序，5-最后登录时间降序】</param>
        /// <returns></returns>
        public Paging<User> FindPageList(Paging<User> pagingUser, int? roleID, string username, string name, int? sex, string email, int? order)
        {
            //查询表达式
#pragma warning disable CS0618 // 类型或成员已过时
            var _where = PredicateBuilder.True<User>();
#pragma warning restore CS0618 // 类型或成员已过时
            if (roleID != null && roleID > 0) _where = _where.And(u => u.RoleID == roleID);
            if (!string.IsNullOrEmpty(username)) _where = _where.And(u => u.Username.Contains(username));
            if (!string.IsNullOrEmpty(name)) _where = _where.And(u => u.Name.Contains(name));
            if (sex != null && sex >= 0 && sex <= 2) _where = _where.And(u => u.Sex == sex);
            if (!string.IsNullOrEmpty(email)) _where = _where.And(u => u.Email.Contains(email));
            //排序
            OrderParam _orderParam;
            switch (order)
            {
                case 0://ID升序
                    _orderParam = new OrderParam() { PropertyName = "UserID", Method = OrderMethod.ASC };
                    break;
                case 1://ID降序
                    _orderParam = new OrderParam() { PropertyName = "UserID", Method = OrderMethod.DESC };
                    break;
                case 2://注册时间降序
                    _orderParam = new OrderParam() { PropertyName = "RegTime", Method = OrderMethod.ASC };
                    break;
                case 3://注册时间升序
                    _orderParam = new OrderParam() { PropertyName = "RegTime", Method = OrderMethod.DESC };
                    break;
                case 4://最后登录时间升序
                    _orderParam = new OrderParam() { PropertyName = "LastLoginTime", Method = OrderMethod.ASC };
                    break;
                case 5://最后登录时间降序
                    _orderParam = new OrderParam() { PropertyName = "LastLoginTime", Method = OrderMethod.DESC };
                    break;
                default://ID降序
                    _orderParam = new OrderParam() { PropertyName = "UserID", Method = OrderMethod.DESC };
                    break;
            }
            pagingUser.Items = Repository.FindPageList(pagingUser.PageSize, pagingUser.PageIndex, out pagingUser.TotalNumber, _where.Expand(), _orderParam).ToList();
            return pagingUser;
        }

        /// <summary>
        /// Email是否存在
        /// </summary>
        /// <param name="email">Email[不区分大小写]</param>
        /// <returns></returns>
        public bool HasEmail(string email)
        {
            return base.Repository.IsContains(u => u.Email.ToUpper() == email.ToUpper());
        }

        /// <summary>
        /// 用户名是否存在
        /// </summary>
        /// <param name="accounts">用户名[不区分大小写]</param>
        /// <returns></returns>
        public bool HasUsername(string username)
        {
            return base.Repository.IsContains(u => u.Username.ToUpper() == username.ToUpper());
        }

    }
}