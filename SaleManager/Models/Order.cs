﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManager.Models
{
    public enum OrderStatus
    {
        Lack = 1, // Đơn hàng mới
        Done, // Đã hủy
    }
    public class Order
    {
        [DisplayName("Mã đơn hàng")]
        public long OrderId { get; set; }

        [ DisplayName("Mã khách hàng")]
        public long CustomerId { get; set; }

        [DisplayName("Ngày bán")]
        [DataType(DataType.Date, ErrorMessage = "{0} không đúng định dạng")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SaleDate { get; set; }

        [DisplayName("Trạng thái")]
        public OrderStatus Status { get; set; }

        [DisplayName("Ghi chú")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [DisplayName("Tổng tiền")]
        public decimal Total { get; set; }

        [DisplayName("Đã trả")]
        public decimal Paid { get; set; }

        [DisplayName("Còn thiếu")]
        public decimal Lack { get; set; }

        [NotMapped]
        public string Details { get; set; }


        public virtual Customer Customer { get; set; }
        public virtual IList<OrderDetail> OrderDetails { get; set; }
    }
}
