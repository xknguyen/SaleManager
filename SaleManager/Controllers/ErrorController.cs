using System.Web.Mvc;

namespace SaleManager.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult ErrorPage(string url)
        {
            ViewBag.Url = url;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult AccessDeniedPage(string url)
        {
            ViewBag.Url = url;
            return View();
        }
    }
}