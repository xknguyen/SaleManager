using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using SaleCore.Utilities;

namespace SaleManager.Models
{
    public class OrderDetail
    {

        public OrderDetail(JToken detail)
        {
            OrderId = DataUtil.ToInt(detail["OrderId"]);
            ProductId = DataUtil.ToLong(detail["ProductId"]);
            Price = DataUtil.ToLong(detail["Price"]);
        }

        public OrderDetail()
        {
            
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long OrderId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ProductId { get; set; }

        public long Price { get; set; }

        public int Quantity { get; set; }
        public string Note { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}