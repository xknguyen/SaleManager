using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManager.Models
{
    public enum OrderStatus
    {
        [Description("Chờ xác nhận")]
        New = 1, // Đơn hàng mới
        [Description("Đã hủy")]
        Cancelled, // Đã hủy
        [Description("Đã xác nhận")]
        Approved, // Đã xác nhận
        [Description("Đang giao hàng")]
        Shipping, // Đang giao hàng
        [Description("Đã trả hàng")]
        Returned, // Trả hàng
        [Description("Thành công")]
        Success // Đã giao hàng thành công
    }
    public class Order
    {
        [DisplayName("Mã đơn hàng")]
        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(128), DisplayName("Mã khách hàng")]
        public string CustomerId { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(128), DisplayName("Mã nhân viên")]
        public string EmployeeId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("Ngày đặt hàng")]
        [DataType(DataType.Date, ErrorMessage = "{0} không đúng định dạng")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OrderDate { get; set; } // Ngày đặt hàng

        [DisplayName("Ngày yêu cầu giao")]
        [DataType(DataType.Date, ErrorMessage = "{0} không đúng định dạng")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequiredDate { get; set; } // Ngày yêu cầu giao

        [ScaffoldColumn(false)]
        [DisplayName("Ngày giao thực tế")]
        [DataType(DataType.Date, ErrorMessage = "{0} không đúng định dạng")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ShipDate { get; set; } // Ngày giao thực tế

        [DisplayName("Phí vận chuyển")]
        public int Freight { get; set; } // Phí vận chuyển

        [DisplayName("Mã công ty vận chuyển")]
        public int? ShipVia { get; set; } // Mã công ty vận chuyển

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(50, ErrorMessage = "{0} không vượt quá 50 kí tự"), DisplayName("Họ tên người nhận")]
        public string ShipName { get; set; } // Họ tên người nhận

        [Required(ErrorMessage = "{0} không được để trống"),
         StringLength(300, ErrorMessage = "{0} không vượt quá 300 kí tự"), DisplayName("Địa chỉ giao hàng")]
        public string ShipAddress { get; set; } // Địa chỉ giao hàng

        [Required(ErrorMessage = "{0} không được để trống"),
         StringLength(20, ErrorMessage = "{0} không vượt quá 20 kí tự"), DisplayName("Số điện thoại")]
        public string ShipTel { get; set; } // Người nhận hàng

        [DisplayName("Trạng thái")]
        public OrderStatus Status { get; set; } // Trạng thái

        [StringLength(500, ErrorMessage = "{0} không vượt quá 500 kí tự"), DisplayName("Ghi chú"),
         DataType(DataType.MultilineText)]
        public string Notes { get; set; } // Các ghi chú

        [DisplayName("Tổng tiền")]
        public decimal Total { get; set; }

        public int? PromotionId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Account Employee { get; set; }
        public virtual Account Customer { get; set; }
        public virtual Shipper Shipper { get; set; }
        public virtual IList<OrderDetail> OrderDetails { get; set; }
        public virtual Promotion Promotion { get; set; }

        public float TotalWeight
        {
            get
            {
                if (OrderDetails != null)
                    return OrderDetails.Sum(item => item.TotalWeight);
                return 0;
            }
        }
    }
}
