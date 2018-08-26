using System.Web.Mvc;

namespace GTWeb.Areas.Control.Controllers
{
    public class HomeController : Controller
    {
        [AdminAuthorize]
        // GET: Control/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}