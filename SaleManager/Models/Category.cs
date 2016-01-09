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
        [Display(Name = "Tên danh mục")]
        public string GroupName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public string GetDescription => HttpUtility.HtmlDecode(Description);

        [Display(Name = "Danh mục cha")]
        public long? ParentGroupId { get; set; }

        public long CourseId { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Actived { get; set; }

        [Display(Name = "Thứ tự")]
        public int OrderNo { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Group ParentGroup { get; set; }
        public virtual IList<Group> ChildGroups { get; set; }
    }
}
