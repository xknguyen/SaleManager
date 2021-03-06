﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace SaleManager.Models
{
    public class Product
    {
        [DisplayName("Mã sản phẩm")]
        public int ProductId { get; set; }      // Mã sản phẩm

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }        // Tên sản phẩm

        [DisplayName("Hình ảnh")]
        public string ThumbImage { get; set; }      // Hình ảnh

        [DisplayName("Số hiệu sản phẩm")]
        public string ProductCode { get; set; }     // Số hiệu sản phẩm

        [DisplayName("Mô tả")]
        [Column(TypeName = "ntext")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }     // Mô tả chi tiết

        [DisplayName("Đơn vị")]
        public string QtyPerUnit { get; set; }      // Đơn vị tính

        [DisplayName("Giá nhập")]
        public int InputPrice { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [DisplayName("Giá bán A")]
        public int PriceA { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [DisplayName("Giá bán B")]
        public int PriceB { get; set; }

        [DisplayName("Mã nhà cung cấp")]
        public long? SupplierId { get; set; }     // Mã nhà cung cấp

        [Required(ErrorMessage = "{0} không được để trống")]
        [DisplayName("Mã danh mục")]
        public long CategoryId { get; set; }

        [DisplayName("Trạng thái")]
        public bool Actived { get; set; }       // Đánh dấu xóa

        [DisplayName("Trọng lượng")]
        public float? Weight { get; set; }

        public virtual IList<OrderDetail> OrderDetails { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Category Category { get; set; }
    }
}
