using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SaleManager.Models
{
    public class Supplier
    {
        [DisplayName("Mã công ty")]
        public long SupplierId { get; set; }     // Mã nhà cung cấp

        [DisplayName("Tên công ty")]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string SupplierName { get; set; }        // Tên công ty

        [DisplayName("Mô tả")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }     // Mô tả sơ lược về công ty

        [DisplayName("Tên người đại diện")]
        public string ContactName { get; set; }     // Tên người đại diện

        [DisplayName("Chức năng người đại diện")]
        public string ContactTitle { get; set; }        // Chức danh người đại diện

        [DisplayName("Địa chỉ")]
        public string Address { get; set; }     // Địa chỉ công ty

        [Display(Name = "Email")]
        public string Email { get; set; }       // Địa chỉ email

        [DisplayName("Số điện thoại")]
        public string Phone { get; set; }       // Số điện thoại

        [DisplayName("Số Fax")]
        public string Fax { get; set; }     // Số Fax

        [DisplayName("Địa chỉ Website")]
        public string HomePage { get; set; }        //  Địa chỉ Website

        public virtual IList<Product> Products { get; set; } 
    }
}
