using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SaleManager.Models
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("Account")]
        [StringLength(128)]
        public string AccountId { get; set; }       // Mã tài khoản

        [StringLength(10, ErrorMessage = "{0} không vượt quá {1} kí tự.")]
        [Display(Name = "Mã tài khoản")]
        public string Identity { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [DisplayName("Họ và tên lót")]
        [StringLength(100, ErrorMessage = "{0} không vượt quá {1} kí tự.")]
        public string LastName { get; set; }       // Họ và tên lót

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(100, ErrorMessage = "{0} không vượt quá {1} kí tự.")]
        [DisplayName("Tên")]
        public string FirstName { get; set; }        // Tên

        [StringLength(1000, ErrorMessage = "{0} không vượt quá {1} kí tự.")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [DisplayName("Ghi chú")]
        public string Notes { get; set; }       // Ghi chú

        [DataType(DataType.Date)]
        [DisplayName("Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Họ tên")]
        public string FullName      // Họ tên đầy đủ
        {
            // ReSharper disable once ConvertPropertyToExpressionBody
            get { return string.Format("{1} {0}", FirstName, LastName); }
        }

        [Display(Name = "Trạng thái")]
        public bool Actived { get; set; }

        public virtual Account Account { get; set; }
    }
}