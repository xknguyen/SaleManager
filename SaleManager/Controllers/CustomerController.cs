using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using SaleCore.Extensions;
using SaleCore.Utilities;
using SaleManager.Models;

namespace SaleManager.Controllers
{
    public class CustomerController : BaseController
    {
        
        /// <summary>
        /// Trang danh sách khách hàng
        /// </summary>
        /// <param name="keyword"> Từ khóa tìm kiếm</param>
        /// <param name="page">trang hiện tại</param>
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="status"> Trạng thái</param>
        /// <returns></returns>
        public ActionResult Index(string keyword, int? page, int? pageSize, int? status)
        {
            var accounts = DbContext.Customers.AsQueryable();

            // chuyển keyword thành số để tìm theo ID
            var keyNumber = DataUtil.ToLong(keyword);

            if (!string.IsNullOrEmpty(keyword))
                accounts = accounts.Where(x => x.FullName.Contains(keyword) ||
                                                 x.CustomerId == keyNumber ||
                                                 x.Notes.Contains(keyword) ||
                                                 x.Address.Contains(keyword) ||
                                                 x.PhoneNumber.Contains(keyword));

            // Nếu chỉ số trang và số mẫu tin trên mỗi trang không được
            // thiết lập thì gán giá trị mặc định tương ứng là 1 và 10.
            if (!page.HasValue || page.Value < 1) page = 1;
            if (!pageSize.HasValue || pageSize < 10) pageSize = 10;

            // thiết lập trạng thái mặc định là 0
            if (!status.HasValue) status = 0;

            // Lưu lại từ khóa, số mẫu tin/trang để hiển thị trên trang web
            ViewBag.Keyword = keyword;
            ViewBag.SupStatus = new SelectList(new List<Object>()
            {
                new {text = "Chọn tất cả", value = 0},
                new {text = "Không nợ", value = 1},
                new {text = "Còn nợ", value = 2}
            }, "value", "text", status);
            ViewBag.PageSize = new SelectList(new[] { 10, 25, 50, 100 }, pageSize);
            ViewBag.CurrentPageSize = pageSize;
            ViewBag.CurrentStatus = status;

            // Sắp tăng các nhà cung cấp theo tên và thực hiện việc phân trang bằng 
            // cách sử dụng thư viện PagedList.MVC
            accounts = accounts.OrderBy(x => x.FullName);

            switch (status.Value)
            {
                case 1:
                    accounts = accounts.Where(x => x.Lack != 0);
                    break;
                case 2:
                    accounts = accounts.Where(x => x.Lack == 0);
                    break;
            }
            var data = accounts.ToPagedList(page.Value, pageSize.Value);
            return View(data);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DbContext.Customers.Add(customer);
                    if (DbContext.SaveChanges() > 0)
                        return Redirect(null);
                    ModelState.AddModelError("", "Đã có lỗi xảy ra. Vui lòng thử lại sau.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ex.Write(LogPath);
            }
            return View(customer);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return RedirectErrorPage(Url.Action("Index", "Customer"));
            }
            var customer = DbContext.Customers.Find(id);
            if (customer == null)
            {
                return RedirectErrorPage(Url.Action("Index", "Customer"));
            }
            return View(customer);
        }

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            var customerDb = DbContext.Customers.Find(customer.CustomerId);
            if (customerDb == null)
            {
                ModelState.AddModelError(string.Empty, "Không thể lưu thay đổi, danh mục này đã bị xóa");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TryUpdateModel(customerDb);
                    DbContext.Entry(customerDb).State = EntityState.Modified;
                    if (DbContext.SaveChanges() > 0)
                    {
                        return Redirect(null);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ex.Write(LogPath);
                }
            }
            return View(customerDb);
        }

        [HttpPost]
        public ActionResult Delete(long? id)
        {
            string message = "";
            bool success = false;

            try
            {
                if (id == null)
                {
                    message = "Thông tin mã khách hàng bị thiếu!";
                }
                else
                {
                    var customer = DbContext.Customers.Find(id);
                    if (customer == null)
                    {
                        message = "Thông tin mã khách hàng không còn tồn tại!";
                    }
                    else
                    {
                        DbContext.Customers.Remove(customer);
                        if (DbContext.SaveChanges() > 0)
                        {
                            message = "Xóa thành công";
                            success = true;
                        }
                        else
                        {
                            message = "Đã có lỗi xảy ra. Vui lòng thử lại sau!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Write(LogPath);
                message = "Đã có lỗi xảy ra. Vui lòng thử lại sau!";
            }

            return Json(new
            {
                Message = message,
                Success = success
            });
        }
    }
}