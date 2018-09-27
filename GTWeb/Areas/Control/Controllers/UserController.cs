using GT.Core;
using GT.Utility;
using GTWeb.Areas.Control;
using GTWeb.Areas.Control.Models;
using GTWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GTWeb.Areas.Control.Controllers
{
    [AdminAuthorize]
    /// <summary>
    /// 用户控制器
    /// </summary>

    public class UserController : Controller
    {
        private UserManager userManager = new UserManager();

        /// <summary>
        /// 默认页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 分页列表【json】
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="username">用户名</param>
        /// <param name="name">名称</param>
        /// <param name="sex">性别</param>
        /// <param name="email">Email</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="order">排序</param>
        /// <returns>Json</returns>
        public ActionResult PageListJson(int? roleID, string username, string name, int? sex, string email, int? pageNumber, int? pageSize, int? order)
        {
            Paging<User> _pagingUser = new Paging<User>();
            if (pageNumber != null && pageNumber > 0) _pagingUser.PageIndex = (int)pageNumber;
            if (pageSize != null && pageSize > 0) _pagingUser.PageSize = (int)pageSize;
            var _paging = userManager.FindPageList(_pagingUser, roleID, username, name, sex, email, null);
            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            //角色列表
            var _roles = new RoleManager().FindList();
            List<SelectListItem> _listItems = new List<SelectListItem>(_roles.Count());
            foreach (var _role in _roles)
            {
                _listItems.Add(new SelectListItem() { Text = _role.Name, Value = _role.RoleID.ToString() });
            }
            ViewBag.Roles = _listItems;
            //角色列表结束
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddUserViewModel userViewModel)
        {
            if (userManager.HasUsername(userViewModel.Username)) ModelState.AddModelError("Username", "用户名已存在");
            if (userManager.HasEmail(userViewModel.Email)) ModelState.AddModelError("Email", "Email已存在");
            if (ModelState.IsValid)
            {
                GT.Core.User _user = new GT.Core.User
                {
                    RoleID = userViewModel.RoleID,
                    Username = userViewModel.Username,
                    Name = userViewModel.Name,
                    Sex = userViewModel.Sex,
                    Password = Security.SHA256(userViewModel.Password),
                    Email = userViewModel.Email,
                    RegTime = System.DateTime.Now
                };
                var _response = userManager.Add(_user);
                if (_response.Code == 1) return View("Prompt", new Prompt()
                {
                    Title = "添加用户成功",
                    Message = "您已成功添加了用户【" + _response.Data.Username + "（" + _response.Data.Name + "）】",
                    Buttons = new List<string> {"<a href=\"" + Url.Action("Index", "User") + "\" class=\"btn btn-default\">用户管理</a>",
                 "<a href=\"" + Url.Action("Details", "User",new { id= _response.Data.UserID }) + "\" class=\"btn btn-default\">查看用户</a>",
                 "<a href=\"" + Url.Action("Add", "User") + "\" class=\"btn btn-default\">继续添加</a>"}
                });
                else ModelState.AddModelError("", _response.Message);
            }
            //角色列表
            var _roles = new RoleManager().FindList();
            List<SelectListItem> _listItems = new List<SelectListItem>(_roles.Count());
            foreach (var _role in _roles)
            {
                _listItems.Add(new SelectListItem() { Text = _role.Name, Value = _role.RoleID.ToString() });
            }
            ViewBag.Roles = _listItems;
            //角色列表结束

            return View(userViewModel);
        }

        /// <summary>
        /// 用户名是否可用
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult CanUsername(string UserName)
        {
            return Json(!userManager.HasUsername(UserName));
        }

        /// <summary>
        /// Email是否存可用
        /// </summary>
        /// <param name="Email">Email</param>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult CanEmail(string Email)
        {
            return Json(!userManager.HasEmail(Email));
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="id">用户主键</param>
        /// <returns>分部视图</returns>
        public ActionResult Modify(int id)
        {
            //角色列表
            var _roles = new RoleManager().FindList();
            List<SelectListItem> _listItems = new List<SelectListItem>(_roles.Count());
            foreach (var _role in _roles)
            {
                _listItems.Add(new SelectListItem() { Text = _role.Name, Value = _role.RoleID.ToString() });
            }
            ViewBag.Roles = _listItems;
            //角色列表结束
            return PartialView(userManager.Find(id));
        }
    }
}