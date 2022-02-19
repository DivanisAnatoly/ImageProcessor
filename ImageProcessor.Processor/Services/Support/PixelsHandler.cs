using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor.Processor.Services.Support
{
    internal class PixelsHandler
    {
        public static Color GetAvgColor(List<Color> pixels)
        {
            int alphaChanal = 0;
            int redChanal = 0;
            int greenChanal = 0;
            int blueChanal = 0;

            foreach (Color pixel in pixels)
            {
                alphaChanal += pixel.A;
                redChanal += pixel.R;
                greenChanal += pixel.G;
                blueChanal += pixel.B;
            }

            alphaChanal = (int)Math.Round(alphaChanal / (pixels.Count * 1.0), MidpointRounding.AwayFromZero);
            redChanal = (int)Math.Round(redChanal / (pixels.Count * 1.0), MidpointRounding.AwayFromZero);
            greenChanal = (int)Math.Round(greenChanal / (pixels.Count * 1.0), MidpointRounding.AwayFromZero);
            blueChanal = (int)Math.Round(blueChanal / (pixels.Count * 1.0), MidpointRounding.AwayFromZero);

            return Color.FromArgb(alphaChanal, redChanal, greenChanal, blueChanal);
        }


        public static Color GetMedianPixel(List<Color> pixels)
        {
            pixels.Sort((Comparison<Color>)(
                (Color left, Color right) =>
                  (left.R * 299 + left.G * 587 + left.B * 114).CompareTo(
                    right.R * 299 + right.G * 587 + right.B * 114)));

            int median = (int)Math.Round(pixels.Count / 2.0);

            return pixels[median];
        }

        public static Color ChangePixelBrightness(Color pixel, int delta)
        {
            int red = pixel.R + delta;
            int green = pixel.G + delta;
            int blue = pixel.B + delta;

            if (red > 255) { red = 255; }
            else if (red < 0) { red = 0; }

            if (green > 255) { green = 255; }
            else if (green < 0) { green = 0; }

            if (blue > 255) { blue = 255; }
            else if (blue < 0) { blue = 0; }

            return Color.FromArgb(pixel.A, red, green, blue);
        }

        public static int GetPixelBrightness(Color pixel)
        {
            return (int)((pixel.R * 0.3) + (pixel.G * 0.59) + (pixel.B * 0.11));
        }
    }
}
