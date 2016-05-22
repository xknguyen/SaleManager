using System;
using System.Linq;
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

        protected virtual bool OnUpdateToggle(
            string propName, bool value, object[] keys)
        {
            return true;
        }

        /// <summary>
        /// Cập nhật giá trị một thuộc tính có kiểu true/false
        /// </summary>
        /// <param name="args">
        /// Chuỗi chứa tên thuộc tính, giá trị hiện tại và id
        /// của mẫu tin cần cập nhật - phân tách nhau bởi dấu _
        /// </param>
        /// <returns>
        /// Trả về đối tượng Json cho biết có cập nhật thành công
        /// hay không. Nếu có, dữ liệu đi kèm sẽ là chuỗi args mới
        /// chứa giá trị sau khi cập nhật. Nếu không, dữ liệu đi
        /// kèm sẽ là thông báo lỗi.
        /// </returns>
        [HttpPost]
        public JsonResult UpdateToggle(string args)
        {
            var success = false; // Cho biết cập nhật thành công?
            var html = string.Empty; // Thông báo trả về cho client

            try
            {
                // Tách chuỗi args thành mảng các chuỗi chứa
                var data = args.Split('_');
                var propName = data[0]; // Tên thuộc tính
                var value = !bool.Parse(data[1]); // Giá trị hiện tại
                var keys = data.Skip(2).ToArray(); // Id của mẫu tin

                // Gọi hàm cập nhật giá trị thuộc tính
                if (OnUpdateToggle(propName, value, keys))
                {
                    success = true;

                    // Tạo chuỗi tương tự args với giá trị mới

                    html = string.Format("{0}_{1}_{2}",
                        propName,
                        value.ToString().ToLower(),
                        string.Join("_", keys));
                }
            }
            catch (Exception ex)
            {
                success = false;
                html = ex.Message; // Lưu lại thông báo lỗi
            }
            return Json(new
            {
                Result = success,
                Message = html
            });
        }

    }
}