using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SaleManager.Models
{
    public class Category
    {
        [Required(ErrorMessage = "{0} không được để trống")]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Danh mục cha")]
        public long? ParentCategoryId { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Actived { get; set; }

        [Display(Name = "Thứ tự")]
        public int OrderNo { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Category ParentCategory { get; set; }
        public virtual IList<Category> ChildCategories { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
