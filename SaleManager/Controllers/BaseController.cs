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

        /// <summary>
        /// Đường dẫn của trang Web
        /// </summary>
        public string LogPath => Server.MapPath("/Log");

        public virtual ActionResult RedirectErrorPage(string url)
        {
            return RedirectToAction("ErrorPage", "Error", new { url });
        }

        /// <summary>
        /// Phương thức chuyển hướng người dùng dụa vào
        /// hàn động mà người dùng chọn trong form cập
        /// nhật hay thêm mới dữ liệu
        /// </summary>
        /// <param name="id">Mã của mẫu tin</param>
        /// <returns>
        /// Chuyển hướng người dùng tới trang Create nếu
        /// người dùng chọn nút Lưu & thêm mới. Nếu người
        /// dùng chọn nút Lưu & đóng thì chuyển hướng tới
        /// trang Index. Nếu chọn nút Lưu & cập nhật hoặc
        /// Tạm lưu thì chuyển hướng tới trang Edit.
        /// </returns>
        public virtual ActionResult Redirect(object id)
        {
            var saveAction = Request.Form["save-action"];
            switch (saveAction)
            {
                case "save-new":
                    return RedirectToAction("Create");

                case "save-edit":
                    return RedirectToAction("Edit", new { id });

                default:
                    return RedirectToAction("Index");
            }
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
            var account = userManager.FindById(id);
            ViewBag.Roles = userManager.GetRoles(id);
            ViewBag.Account = account;
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