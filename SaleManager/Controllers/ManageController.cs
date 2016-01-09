using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using SaleManager.Models;

namespace SaleManager.Controllers
{
    public class ManageController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("doi-mat-khau.html")]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {

            var userId = User.Identity.GetUserId();
            var userName = User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return View();
            }
            var userManager = new UserManager<Account>(new UserStore<Account>(DbContext));
            var result = userManager.ChangePassword(userId, model.OldPassword, model.Password);

            if (result.Succeeded)
            {
                SignInManager.PasswordSignInAsync(
                userName,
                model.Password,
                false,
                shouldLockout: false);
                return RedirectToAction("Index", "Home"); ;
            }
            AddErrors(result);
            return View();
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}