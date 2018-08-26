using System.Web;
using System.Web.Mvc;

namespace GTWeb.Areas.Control
{
    /// <summary>
    /// 管理员身份验证
    /// <remarks>
    ///创建：2014年12月16日
    /// </remarks>
    /// </summary>
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool _pass = false;
            if (httpContext.Session["AdminID"] != null) _pass = true;
            return _pass;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Control/Admin/Login");
        }
    }
}