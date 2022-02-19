using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessor.Processor.Algorithms.ObjectDetection
{
    internal static class Clusterization
    {
        public static Bitmap ImageSegment(this Bitmap image, int x, int y)
        {
            int w = image.Width;
            int h = image.Height;
            BitmapData image_data = image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            image.UnlockBits(image_data);
            //limit the color range for segmentation
            int d0 = 30;
            int sample_position = x * 3 + y * image_data.Stride;
            for (int i = 0; i < bytes - 3; i += 3)
            {
                double euclidean = 0;
                for (int c = 0; c < 3; c++)
                {
                    euclidean += Math.Pow(buffer[i + c] - buffer[sample_position + c], 2);
                }
                euclidean = Math.Sqrt(euclidean);
                for (int c = 0; c < 3; c++)
                {
                    result[i + c] = (byte)(euclidean > d0 ? 0 : 255);
                }
            }
            Bitmap res_img = new(w, h);
            BitmapData res_data = res_img.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            Marshal.Copy(result, 0, res_data.Scan0, bytes);
            res_img.UnlockBits(res_data);
            return res_img;
        }
    }

}
