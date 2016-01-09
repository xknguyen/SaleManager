using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SaleManager.DAL;
using SaleManager.Models;

namespace SaleManager.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected SaleDbContext DbContext = new SaleDbContext();
        
        public virtual ActionResult RedirectErrorPage(string url)
        {
            return RedirectToAction("ErrorPage", "Error", new { url });
        }

        public virtual ActionResult RedirectAccessDeniedPage(string url)
        {
            return RedirectToAction("AccessDeniedPage", "Error", new { url });
        }
        private void CreateView()
        {
            var userManager = new UserManager<Account>(new UserStore<Account>(DbContext));
            var id = User.Identity.GetUserId();
            ViewBag.Roles = userManager.GetRoles(id);
        }

        protected override ViewResult View(IView view, object model)
        {
            CreateView();
            return base.View(view, model);
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            CreateView();
            return base.View(viewName, masterName, model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                DbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}