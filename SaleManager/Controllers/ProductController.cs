using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using SaleCore.Extensions;
using SaleCore.Utilities;
using SaleManager.Models;

namespace SaleManager.Controllers
{
    public class ProductController : BaseController
    {
        public ActionResult Index(string keyword, int? page, int? pageSize, long? cateId, long? suppId, int? status)
        {
            var products = DbContext.Products.AsQueryable();

            if (cateId.HasValue && cateId != 0)
            {
                products = products.Where(s => s.CategoryId == cateId);
                ViewBag.CateId = cateId;
            }
            else
            {
                ViewBag.CateId = 0;
            }

            if (suppId.HasValue && suppId != 0)
            {
                products = products.Where(s => s.SupplierId == suppId);
                ViewBag.SuppId = suppId;
            }
            else
            {
                ViewBag.SuppId = 0;
            }

            // chuyển keyword thành số để tìm theo ID
            var keyNumber = DataUtil.ToLong(keyword);

            if (!string.IsNullOrEmpty(keyword))
                products = products.Where(x => x.ProductCode.Contains(keyword) ||
                                                 x.ProductId == keyNumber ||
                                                 x.ProductName.Contains(keyword) ||
                                                 x.Description.Contains(keyword) ||
                                                 x.InputPrice == keyNumber ||
                                                 x.PriceA == keyNumber ||
                                                 x.PriceB == keyNumber ||
                                                 x.Description.Contains(keyword) ||
                                                 x.QtyPerUnit.Contains(keyword) ||
                                                 x.Supplier.SupplierName.Contains(keyword) ||
                                                 x.Category.CategoryName.Contains(keyword));

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
                new {text = "Đang hoạt động", value = 1},
                new {text = "Tạm khóa", value = 2}
            }, "value", "text", status);
            ViewBag.PageSize = new SelectList(new[] { 10, 25, 50, 100 }, pageSize);
            ViewBag.CurrentPageSize = pageSize;
            ViewBag.CurrentStatus = status;

            // Sắp tăng các nhà cung cấp theo tên và thực hiện việc phân trang bằng 
            // cách sử dụng thư viện PagedList.MVC
            products = products.OrderBy(x => x.ProductId);

            switch (status.Value)
            {
                case 1:
                    products = products.Where(x => x.Actived);
                    break;
                case 2:
                    products = products.Where(x => !x.Actived);
                    break;
            }
            var data = products.ToPagedList(page.Value, pageSize.Value);
            return View(data);
        }

        private void InitFormData(Product product)
        {
            //Lấy tất cả các nhóm câu hỏi và gom nhóm chúng
            var categories = DbContext.Categories.Where(s => s.Actived).ToList();
            var suppliers = DbContext.Suppliers.ToList();

            //Tạo danh sách chọn làm dữ liệu nguồn cho DropDownList
            ViewBag.CategoryId = product.CategoryId > 0
                ? new SelectList(categories, "CategoryId", "CategoryName", product.CategoryId)
                : new SelectList(categories, "CategoryId", "CategoryName", null);
            ViewBag.SupplierId = product.SupplierId > 0
                 ? new SelectList(suppliers, "SupplierId", "SupplierName", product.SupplierId)
                 : new SelectList(suppliers, "SupplierId", "SupplierName", null);
        }

        protected override bool OnUpdateToggle(string propName, bool value, object[] keys)
        {
            string query = string.Format("UPDATE dbo.Products SET {0} = @p0 WHERE ProductId = @p1", propName);
            return DbContext.Database.ExecuteSqlCommand(query, value, keys[0]) > 0;
        }

        public ActionResult Create()
        {
            var product = new Product() {Actived = true};
            InitFormData(product);
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DbContext.Products.Add(product);
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
            InitFormData(product);
            return View(product);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return RedirectErrorPage(Url.Action("Index", "Product"));
            }
            var product = DbContext.Products.Find(id);
            if (product == null)
            {
                return RedirectErrorPage(Url.Action("Index", "Product"));
            }
            InitFormData(product);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            var productDb = DbContext.Products.Find(product.ProductId);
            if (productDb == null)
            {
                ModelState.AddModelError(string.Empty, "Không thể lưu thay đổi, sản phẩm này đã bị xóa");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TryUpdateModel(productDb);
                    DbContext.Entry(productDb).State = EntityState.Modified;
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
            InitFormData(product);
            return View(product);
        }

        //[HttpPost]
        //public ActionResult Delete(long? id)
        //{
        //    string message;
        //    bool success = false;

        //    try
        //    {
        //        if (!id.HasValue)
        //        {
        //            message = "Thông tin mã sản phẩm bị thiếu!";
        //        }
        //        else
        //        {
        //            var product = DbContext.Products.Find(id);
        //            if (product == null)
        //            {
        //                message = "Thông tin sản phẩm không còn tồn tại!";
        //            }
        //            else
        //            {
        //                foreach (var detail in product.OrderDetails)
        //                {
        //                    detail.Note = string.IsNullOrEmpty(detail.Note) ? "" : detail.Note;
        //                    detail.Note += "Tên sản phẩm: " + product.ProductName + ".";
        //                }

        //                DbContext.Products.Remove(product);
        //                if (DbContext.SaveChanges() > 0)
        //                {
        //                    message = "Xóa thành công";
        //                    success = true;
        //                }
        //                else
        //                {
        //                    message = "Đã có lỗi xảy ra. Vui lòng thử lại sau!";
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Write(LogPath);
        //        message = "Đã có lỗi xảy ra. Vui lòng thử lại sau!";
        //    }

        //    return Json(new
        //    {
        //        Message = message,
        //        Success = success
        //    });
        //}
    }
}