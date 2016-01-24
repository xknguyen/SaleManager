using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManager.Models
{
    public class Customer
    {
        public long CustomerId { get; set; }   

        [Required(ErrorMessage = "{0} không được để trống")]
        [DisplayName("Họ và tên")]
        public string FullName { get; set; }  

        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [DisplayName("Số điện thoại")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [DisplayName("Ghi chú")]
        public string Notes { get; set; }

        public decimal Lack => 0;

        public virtual IList<Order> Orders { get; set; }
    }
}