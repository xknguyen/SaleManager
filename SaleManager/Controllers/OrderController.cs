using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SaleCore.Extensions;
using SaleCore.Utilities;
using SaleManager.Models;

namespace SaleManager.Controllers
{
    public class OrderController : BaseController
    {
        public ActionResult Index(string keyword, int? page, int? pageSize, int? status)
        {
            var orders = DbContext.Orders.AsQueryable();

            // chuyển keyword thành số để tìm theo ID
            var keyNumber = DataUtil.ToLong(keyword);

            if (!string.IsNullOrEmpty(keyword))
                orders = orders.Where(x => x.Customer.FullName.Contains(keyword) ||
                                                 x.OrderId == keyNumber ||
                                                 x.Notes.Contains(keyword));

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
                new {text = "Còn thiếu", value = 1},
                new {text = "Thanh toán hết", value = 2}
            }, "value", "text", status);
            ViewBag.PageSize = new SelectList(new[] { 10, 25, 50, 100 }, pageSize);
            ViewBag.CurrentPageSize = pageSize;
            ViewBag.CurrentStatus = status;

            // Sắp tăng các nhà cung cấp theo tên và thực hiện việc phân trang bằng 
            // cách sử dụng thư viện PagedList.MVC
            orders = orders.OrderBy(x => x.OrderId);

            switch (status.Value)
            {
                case 1:
                    orders = orders.Where(x => x.Status == OrderStatus.Lack);
                    break;
                case 2:
                    orders = orders.Where(x => x.Status == OrderStatus.Done);
                    break;
            }
            var data = orders.ToPagedList(page.Value, pageSize.Value);
            return View(data);
        }

        private void InitFormData(Order order)
        {

        }
        private void InitFormData(OrderModel order)
        {
            var customers = DbContext.Customers.Select(x => new { CustomerId = x.CustomerId, FullName = x.CustomerId + " - " + x.FullName }).ToList();
            order.Customers = new SelectList(customers, "CustomerId", "FullName", order.CustomerId);
            var methods = new[]
            {
                new { value = "1", text = "Thanh toán trực tiếp"},
                new { value = "2", text = "Thanh toán nợ"}

            };
            order.MethodofPayments = new SelectList(methods, "value", "text", order.MethodofPayment);
            var product = DbContext.Products.Select(x => new { ProductId = x.ProductId + "-" + x.PriceA + "-" + x.PriceB, ProductText = x.ProductId + " - " + x.ProductName }).ToList();
            order.Products = new SelectList(product, "ProductId", "ProductText", null);
        }

        public ActionResult Create()
        {
            var model = new OrderModel() { SaleDate = DateTime.Now.ToString("dd/MM/yyyy") };
            InitFormData(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(OrderModel order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //DbContext.Orders.Add(order);
                    //if (DbContext.SaveChanges() > 0)
                    //    return Redirect(null);
                    //ModelState.AddModelError("", "Đã có lỗi xảy ra. Vui lòng thử lại sau.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ex.Write(LogPath);
            }
            InitFormData(order);
            return View(order);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return RedirectErrorPage(Url.Action("Index", "Order"));
            }
            var order = DbContext.Orders.Find(id);
            if (order == null)
            {
                return RedirectErrorPage(Url.Action("Index", "Order"));
            }
            InitFormData(order);
            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(Order order)
        {
            var productDb = DbContext.Orders.Find(order.OrderId);
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
            InitFormData(order);
            return View(order);
        }


    }
}