using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace SaleCore.DataAnnotations
{
    public enum ImageValidationResult
    {
        // Kiểu nội dung tập tin không phải là hình ảnh
        InvalidMimeType,

        // Định dạng ảnh không được phép upload
        NotAllowedType,

        // Tập tin không phải là tập tin ảnh
        InvalidHeader,

        // Ảnh có kích cỡ vượt quá quy định
        OverSize,

        // Hợp lệ
        Valid
    }

    /// <summary>
    /// Thuộc tính quy định kích thước tối đa của tập tin
    /// hình ảnh được phép upload.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ImageSizeAttribute : ValidationAttribute
    {
        private ImageValidationResult _ivResult = ImageValidationResult.Valid;

        /// <summary>
        /// Mảng lưu các định dạng nội dung tập tin hình ảnh
        /// </summary>
        private static string[] mimeTypes =
        {
            "image/jpg", "image/jpeg", "image/pjpeg", "image/gif",
            "image/x-png", "image/png", "image/bmp", "image/x-icon",
            "image/x-tiff", "image/tiff"
        };

        /// <summary>
        /// Mảng lưu các loại hình ảnh được phép upload
        /// </summary>
        private static string[] imageExts =
        {
            ".jpg", ".jpeg", ".gif", ".png", ".bmp", ".ico", ".tif", ".tiff"
        };

        /// <summary>
        /// Chiều rộng tối đa, tính theo pixel
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Chiều cao tối đa, tính theo pixel
        /// </summary>
        public int Height { get; set; }

        // Sử dụng {WIDTH} và {HEIGHT} để đánh dấu vị trí
        // sẽ thay thế bởi độ rộng và chiều cao tối đa
        public ImageSizeAttribute()
            : base("Kích thước hình vượt quá cỡ {WIDTH}x{HEIGHT}")
        {
        }

        /// <summary>
        /// Kiểm tra kiểu nội dung của tập tin được upload
        /// có phải là kiểu nội dung hình ảnh
        /// </summary>
        /// <param name="upload">Tập tin được load</param>
        /// <returns>
        /// Trả về false nếu không đúng định dạng nội dung
        /// của các loại tập tin hình ảnh. True nếu ngược lại.
        /// </returns>
        private bool CheckMimeTypes(HttpPostedFileBase upload)
        {
            // Lấy kiểu nội dung cho tập tin được upload
            var contentType = upload.ContentType.ToLower();

            // Nếu kiểu nội dung không thuộc kiểu hình ảnh
            if (!mimeTypes.Contains(contentType))
            {
                // Thì đánh dấu MIME Type không hợp lệ
                _ivResult = ImageValidationResult.InvalidMimeType;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra phần tên mở rộng của tập tin có nằm trong
        /// danh sách những loại hình ảnh được phép upload?
        /// </summary>
        /// <param name="upload">Tập tin được upload</param>
        /// <returns>
        /// Trả về false nếu tên mở rộng không nằm trong danh sách
        /// các định dạng được phép upload. True nếu ngược lại.
        /// </returns>
        private bool CheckFileExtension(HttpPostedFileBase upload)
        {
            // Lấy phần mở rộng của tập tin
            var fileExt = Path.GetExtension(upload.FileName);

            // Nếu có phần tên mở rộng
            if (!string.IsNullOrWhiteSpace(fileExt))
            {
                // Trả về true nếu phần mở rộng nằm trong danh sách cho phép
                if (imageExts.Contains(fileExt, StringComparer.OrdinalIgnoreCase))
                    return true;
            }

            // Đánh dấu định dạng file không hợp lệ
            _ivResult = ImageValidationResult.NotAllowedType;

            return false;
        }

        /// <summary>
        /// Kiểm tra kích thước của file ảnh được upload có vượt
        /// quá khổ quy định bởi 2 thuộc tính Width và Height?
        /// </summary>
        /// <param name="upload">Tâp tin ảnh được upload</param>
        /// <returns>Trả về false nếu kích thước ảnh quá lớn</returns>
        private bool CheckImageSize(HttpPostedFileBase upload)
        {
            // Nếu không thể đọc được nội dung thì gán không hợp lệ
            if (!upload.InputStream.CanRead)
            {
                _ivResult = ImageValidationResult.InvalidHeader;
                return false;
            }
            try
            {
                // Tạo hình ảnh từ luồng upload
                using (var image = Image.FromStream(upload.InputStream))
                {
                    // Kiểm tra kích cỡ ảnh có vượt quá cỡ cho phép
                    // Nếu có, trả về false.
                    if (image.Width > Width || image.Height > Height)
                    {
                        _ivResult = ImageValidationResult.OverSize;
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                _ivResult = ImageValidationResult.InvalidHeader;
                return false;
            }
        }

        public override bool IsValid(object value)
        {
            // Lấy đối tượng lưu tập tin được upload
            var upload = value as HttpPostedFileBase;

            // Nếu không có tập tin được post lên thì xem như hợp lệ
            if (upload == null) return true;

            // Lần lượt thực hiện các thao tác kiểm tra

            // Kiểm tra mime types
            var valid = CheckMimeTypes(upload);

            // Kiểm tra phần đuôi mở rộng
            if (valid) valid = CheckFileExtension(upload);

            // Kiểm tra header có đúng định dạng ảnh
            // if(valid) valid = CheckFileExtension(upload);

            // Kiểm tra hình đó đúng kích cỡ
            if (valid) valid = CheckImageSize(upload);

            return valid;
        }

        public override string FormatErrorMessage(string name)
        {
            var errorMessage = ErrorMessageString;

            switch (_ivResult)
            {
                case ImageValidationResult.InvalidHeader:
                    return "Không thể đọc được nội dung file ảnh";

                case ImageValidationResult.InvalidMimeType:
                case ImageValidationResult.NotAllowedType:
                    return "Hệ thống không hỗ trợ định dạng ảnh này.";

                case ImageValidationResult.OverSize:
                    if (errorMessage != null)
                    {
                        if (errorMessage.Contains("{WIDTH}"))
                            errorMessage = errorMessage.Replace("{WIDTH}", Height.ToString());
                    }
                    // ReSharper disable once AssignNullToNotNullAttribute
                    return errorMessage;

                default:
                    return errorMessage;
            }
        }
    }
}
