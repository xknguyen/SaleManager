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
    public class CategoryController : BaseController
    {
        /// <summary>
        /// Trang danh sách danh mục
        /// </summary>
        /// <param name="keyword"> Từ khóa tìm kiếm</param>
        /// <param name="page">trang hiện tại</param>
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="status"> Trạng thái</param>
        /// <param name="parentId"> Lọc theo nhóm cha</param>
        /// <returns></returns>
        public ActionResult Index(string keyword, int? page, int? pageSize, int? status, long? parentId)
        {
            var categories = DbContext.Categories.AsQueryable();


            if (parentId.HasValue && parentId != 0)
            {
                ViewBag.ParentId = parentId;
                categories = categories.Where(s => s.ParentCategoryId == parentId);
            }
            else
            {
                ViewBag.ParentId = 0;
            }

            // chuyển keyword thành số để tìm theo ID
            var keyNumber = DataUtil.ToLong(keyword);

            if (!string.IsNullOrEmpty(keyword))
                categories = categories.Where(x => x.CategoryName.Contains(keyword) ||
                                                 x.CategoryId == keyNumber ||
                                                 x.Description.Contains(keyword));

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
                new {text = "Danh mục chính", value = 1},
                new {text = "Danh mục còn hoạt động", value = 2},
                new {text = "Danh mục không hoạt động", value = 3}
            }, "value", "text", status);
            ViewBag.PageSize = new SelectList(new[] { 10, 25, 50, 100 }, pageSize);
            ViewBag.CurrentPageSize = pageSize;
            ViewBag.CurrentStatus = status;

            // Sắp tăng các nhà cung cấp theo tên và thực hiện việc phân trang bằng 
            // cách sử dụng thư viện PagedList.MVC
            categories = categories.OrderBy(x => x.CategoryId);

            switch (status.Value)
            {
                case 1:
                    categories = categories.Where(s => s.ParentCategoryId == 0 || s.ParentCategoryId == null);
                    break;
                case 2:
                    categories = categories.Where(x => x.Actived);
                    break;
                case 3:
                    categories = categories.Where(x => !x.Actived);
                    break;
            }
            var data = categories.ToPagedList(page.Value, pageSize.Value);
            return View(data);
        }



        public ActionResult Create(long? parentId)
        {
            Category cate = new Category {Actived = true};
            if (parentId.HasValue)
                cate.ParentCategoryId = parentId;
            InitFormData(cate);
            return View(cate);
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DbContext.Categories.Add(category);
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
            InitFormData(category);
            return View(category);
        }


        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return RedirectErrorPage(Url.Action("Index", "Category"));
            }
            var category = DbContext.Categories.Find(id);
            if (category == null)
            {
                return RedirectErrorPage(Url.Action("Index", "Category"));
            }
            InitFormData(category);
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            var categoryDb = DbContext.Categories.Find(category.CategoryId);
            if (categoryDb == null)
            {
                ModelState.AddModelError(string.Empty, "Không thể lưu thay đổi, danh mục này đã bị xóa");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TryUpdateModel(categoryDb);
                    DbContext.Entry(categoryDb).State = EntityState.Modified;
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
            InitFormData(categoryDb);
            return View(categoryDb);
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
                    message = "Thông tin mã danh mục bị thiếu!";
                }
                else
                {
                    var category = DbContext.Categories.Find(id);
                    if (category == null)
                    {
                        message = "Thông tin mã danh mục không còn tồn tại!";
                    }
                    else
                    {
                        foreach (var child in category.ChildCategories)
                        {
                            child.ParentCategoryId = null;
                        }
                        DbContext.Categories.Remove(category);
                        //DbContext.Entry(category).State = EntityState.Deleted;
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


        private void InitFormData(Category category)
        {
            //Lấy tất cả các nhóm câu hỏi và gom nhóm chúng
            var cates = DbContext.Categories.Where(s => s.Actived && 
            (s.ParentCategoryId == 0 || s.ParentCategoryId==null) && 
            (s.CategoryId != category.CategoryId)).ToList();

            //Tạo danh sách chọn làm dữ liệu nguồn cho DropDownList
            ViewBag.ParentCategoryId = category.ParentCategoryId > 0 
                ? new SelectList(cates, "CategoryId", "CategoryName", category.ParentCategoryId) 
                : new SelectList(cates, "CategoryId", "CategoryName", null);
        }

        protected override bool OnUpdateToggle(string propName, bool value, object[] keys)
        {
            string query = string.Format("UPDATE dbo.Categories SET {0} = @p0 WHERE CategoryId = @p1", propName);
            return DbContext.Database.ExecuteSqlCommand(query, value, keys[0]) > 0;
        }
    }
}