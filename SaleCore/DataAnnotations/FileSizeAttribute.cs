using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SaleCore.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class FileSizeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Thuộc tính quy định dung lượng tối đa của tập tin
        /// được phép upload lên server.
        /// </summary>
        private readonly int _maxSize;

        // maxSize = Dung lượng tối đa, tính theo Megabytes
        // Sử dụng {MAXSIZE} để đánh dấu sẽ thay bằng giá trị maxSize
        public FileSizeAttribute(int maxSize)
            : base("Dung lượng tập tin không được quá {MAXSIZE} MB.")
        {
            _maxSize = maxSize;
        }

        public override bool IsValid(object value)
        {
            // Lấy đối tượng lưu tập tin được upload
            var upload = value as HttpPostedFileBase;

            // Nếu không có tập tin được post lên thì xem như hợp lệ
            if (upload == null) return true;

            // Nếu có, kiểm tra dung lượng có hợp lệ
            // Vì ContentLength tính theo byte nên phải nhân
            // maxSize với 1024 * 1024 để ra số bytes.
            return upload.ContentLength < _maxSize * 1024 * 1024;
        }

        public override string FormatErrorMessage(string name)
        {
            var errorMessage = ErrorMessageString;

            if (errorMessage != null && errorMessage.Contains("{MAXSIZE}"))
                errorMessage = errorMessage.Replace(
                    "{MAXSIZE}", _maxSize.ToString());

            // ReSharper disable once AssignNullToNotNullAttribute
            return errorMessage;
        }
    }
}
