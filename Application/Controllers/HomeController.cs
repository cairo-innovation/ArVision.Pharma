using ArVision.Pharma.Shared.DataModels;
using System.Web.Mvc;

namespace Pharma.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult Login1()
        {
            ViewBag.Title = "Login Page";

            return View();
        }
        [HttpPost]
        public JsonResult Logon(string username, string password)
        {
            //aboziad//should be replace by a service call to validate the user 
            var user = new User{ Id=0, Name = "Admin"};
            if (user!=null)
            {
                return Json(user);
            }
            return Json(new { });
        }
    }
}
