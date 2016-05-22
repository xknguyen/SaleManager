using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using SaleCore.Extensions;
using SaleCore.Utilities;
using SaleManager.Models;

namespace SaleManager.Controllers
{
    public class SupplierController : BaseController
    {
        public ActionResult Index(string keyword, int? page, int? pageSize)
        {
            var supplier = DbContext.Suppliers.AsQueryable();

            // chuyển keyword thành số để tìm theo ID
            var keyNumber = DataUtil.ToLong(keyword);

            if (!string.IsNullOrEmpty(keyword))
                supplier = supplier.Where(x => x.ContactName.Contains(keyword) ||
                                                 x.SupplierId == keyNumber ||
                                                 x.SupplierName.Contains(keyword) ||
                                                 x.Address.Contains(keyword) ||
                                                 x.Description.Contains(keyword) ||
                                                 x.ContactTitle.Contains(keyword) ||
                                                 x.Phone.Contains(keyword) ||
                                                 x.Fax.Contains(keyword) ||
                                                 x.HomePage.Contains(keyword));

            // Nếu chỉ số trang và số mẫu tin trên mỗi trang không được
            // thiết lập thì gán giá trị mặc định tương ứng là 1 và 10.
            if (!page.HasValue || page.Value < 1) page = 1;
            if (!pageSize.HasValue || pageSize < 10) pageSize = 10;

            // thiết lập trạng thái mặc định là 0
            // Lưu lại từ khóa, số mẫu tin/trang để hiển thị trên trang web
            ViewBag.Keyword = keyword;
            ViewBag.PageSize = new SelectList(new[] { 10, 25, 50, 100 }, pageSize);
            ViewBag.CurrentPageSize = pageSize;

            // Sắp tăng các nhà cung cấp theo tên và thực hiện việc phân trang bằng 
            // cách sử dụng thư viện PagedList.MVC
            supplier = supplier.OrderBy(x => x.SupplierId);

            var data = supplier.ToPagedList(page.Value, pageSize.Value);
            return View(data);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DbContext.Suppliers.Add(supplier);
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
            return View(supplier);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return RedirectErrorPage(Url.Action("Index", "Supplier"));
            }
            var supplier = DbContext.Suppliers.Find(id);
            if (supplier == null)
            {
                return RedirectErrorPage(Url.Action("Index", "Supplier"));
            }
            return View(supplier);
        }

        [HttpPost]
        public ActionResult Edit(Supplier supplier)
        {
            var supplierDb = DbContext.Suppliers.Find(supplier.SupplierId);
            if (supplierDb == null)
            {
                ModelState.AddModelError(string.Empty, "Không thể lưu thay đổi, nhà cung cấp này đã bị xóa");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TryUpdateModel(supplierDb);
                    DbContext.Entry(supplierDb).State = EntityState.Modified;
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
            return View(supplierDb);
        }

        [HttpPost]
        public ActionResult Delete(long? id)
        {
            string message;
            bool success = false;

            try
            {
                if (!id.HasValue)
                {
                    message = "Thông tin mã khách hàng bị thiếu!";
                }
                else
                {
                    var supplier = DbContext.Suppliers.Find(id);
                    if (supplier == null)
                    {
                        message = "Thông tin mã khách hàng không còn tồn tại!";
                    }
                    else
                    {
                        foreach (var product in supplier.Products)
                        {
                            product.SupplierId = null;
                        }

                        DbContext.Suppliers.Remove(supplier);
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