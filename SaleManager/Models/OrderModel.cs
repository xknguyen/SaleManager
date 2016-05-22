using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace SaleManager.Models
{
    public class OrderModel
    {
        public long OrderId { get; set; }

        [Required(ErrorMessage = "Chưa chọn khách hàng!")]
        [Display(Name = "Khách hàng(*):")]
        public long CustomerId { get; set; }

        [Display(Name = "Phương thức:")]
        public int? MethodofPayment { get; set; }

        [Display(Name = "Ngày bán:")]
        public string SaleDate { get; set; }

        public string OrderDetailJson { get; set; }

        public List<OrderDetail> OrderDetails
        {
            get
            {
                if (string.IsNullOrEmpty(OrderDetailJson))
                    return new List<OrderDetail>();
                var jObject = JObject.Parse(OrderDetailJson);
                return jObject["OrderDetails"].ToArray().Select(choice => new OrderDetail(choice)).ToList();
            }
        }

        public SelectList Customers { get; set; }
        public SelectList MethodofPayments { get; set; }
        public SelectList Products { get; set; }
    }
}
