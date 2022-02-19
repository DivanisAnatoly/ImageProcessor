using System.Drawing;
using System.IO;

namespace ImageProcessor.Processor.Services.Support
{
    internal class Conventor
    {
        public static Bitmap BytesToBitmap(byte[] image)
        {
            using MemoryStream ms = new(image);
            return new Bitmap(ms);
        }

        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            Bitmap cloneBitmap = new(bitmap);
            bitmap.Dispose();
            byte[] data = default;
            using (MemoryStream ms = new())
            {
                cloneBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                data = ms.ToArray();
            }
            return data;
        }
    }
}
