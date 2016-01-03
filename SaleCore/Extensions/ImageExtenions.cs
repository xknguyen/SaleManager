using System.Drawing;

namespace SaleCore.Extensions
{
    public static class ImageExtenions
    {
        public static Image Resize(this Image image, Size size)
        {
            return new Bitmap(image, size);
        }

    }
}
