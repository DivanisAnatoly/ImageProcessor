using System.IO;
using System.Windows.Media.Imaging;

namespace ImageProcessor.UI.Image
{
    public class ImageProperties
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Extension { get; private set; }



        public ImageProperties(BitmapImage bitmap)
        {
            Width = bitmap.PixelWidth;
            Height = bitmap.PixelHeight;
            Extension = Path.GetExtension(bitmap.UriSource.LocalPath);

        }
    }
}
