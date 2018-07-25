using System.Web.Mvc;
using TaskOperator.Core;

namespace TaskOperator.Web.Controllers
{
    public class HomeController : TaskOperatorController
    {
        public const string ControllerName = "Home";

        public const string IndexAction = "Index";

        public ActionResult Index()
        {
            ViewBag.IsAuthorized = UserPrincipal.CurrentPrincipal == UserPrincipal.Empty ? "0" : "1";
            return View();
        }

        [HttpPost]
        public PartialViewResult GetIntro()
        {
            return PartialView("_Intro");
        }
    }
}
