using ImageProcessor.Processor.Algorithms.ObjectDetection;
using ImageProcessor.Processor.Contracts;
using ImageProcessor.Processor.Interfaces;
using ImageProcessor.Processor.Services.Support;
using ImageProcessor.Processor.Services.Support.MathModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ImageProcessor.Processor.Services
{
    public class ImageFiltersService : IImageFiltersService
    {
        private IImageInfoService _imageInfoService = new ImageInfoService();
        private readonly ImageInfoService imageInfoService = new();
        private Bitmap _imageBitmap;


        public byte[] Binarize(byte[] image, int level, Color mainColor, Color backgroundColor)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);

            if (_imageBitmap == null) { return null; }

            Color curColor;
            int ret;

            if (level > 255) { level = 255; }
            if (level < 0) { level = 0; }

            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    curColor = _imageBitmap.GetPixel(iX, iY);

                    ret = (int)((curColor.R * 0.3) + (curColor.G * 0.59) + (curColor.B * 0.11));
                    curColor = (ret >= level) ? mainColor : backgroundColor;

                    _imageBitmap.SetPixel(iX, iY, curColor);
                }
            }

            return Conventor.BitmapToBytes(_imageBitmap);
        }


        public byte[] ChangeBrightness(byte[] originImage, int brightnessChange)
        {
            _imageBitmap = Conventor.BytesToBitmap(originImage);

            if (_imageBitmap == null) { return null; }

            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    Color curPixelColor = _imageBitmap.GetPixel(iX, iY);
                    Color newPixelColor = PixelsHandler.ChangePixelBrightness(curPixelColor, brightnessChange);

                    _imageBitmap.SetPixel(iX, iY, newPixelColor);
                }
            }

            return Conventor.BitmapToBytes(_imageBitmap);
        }


        public byte[] ChangeContrast(byte[] originImage, int contrastChange)
        {
            _imageBitmap = Conventor.BytesToBitmap(originImage);

            if (_imageBitmap == null) { return null; }

            double contrast = (100.0 + contrastChange) / 100.0;
            contrast *= contrast;

            BrightnessDistribution distribution = imageInfoService.GetBrightnessDistributionInfo(originImage);

            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    Color curPixelColor = _imageBitmap.GetPixel(iX, iY);
                    Color newPixelColor = ChangePixelContrast(curPixelColor, contrast, distribution);

                    _imageBitmap.SetPixel(iX, iY, newPixelColor);
                }
            }

            return Conventor.BitmapToBytes(_imageBitmap);
        }


        private static Color ChangePixelContrast(Color pixel, double contrast, BrightnessDistribution distribution)
        {
            int red = pixel.R;
            int green = pixel.G;
            int blue = pixel.B;
            double bright;

            double redAvg = distribution.RAvgBrightness / 255.0;
            double greenAvg = distribution.GAvgBrightness / 255.0;
            double blueAvg = distribution.BAvgBrightness / 255.0;


            bright = ((((red / 255.0) - redAvg) * contrast) + redAvg) * 255;
            if (bright < 0) { bright = 0; }
            else if (bright > 255) { bright = 255; }
            red = (int)bright;

            bright = ((((green / 255.0) - greenAvg) * contrast) + greenAvg) * 255;
            if (bright < 0) { bright = 0; }
            else if (bright > 255) { bright = 255; }
            green = (int)bright;

            bright = ((((blue / 255.0) - blueAvg) * contrast) + blueAvg) * 255;
            if (bright < 0) { bright = 0; }
            else if (bright > 255) { bright = 255; }
            blue = (int)bright;

            return Color.FromArgb(pixel.A, red, green, blue);
        }


        public byte[] Grayscale(byte[] image)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }

            Color curColor;
            int ret;

            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    curColor = _imageBitmap.GetPixel(iX, iY);

                    ret = (int)((curColor.R * 0.3) + (curColor.G * 0.59) + (curColor.B * 0.11));
                    curColor = Color.FromArgb(ret, ret, ret, ret);

                    _imageBitmap.SetPixel(iX, iY, curColor);
                }
            }

            return Conventor.BitmapToBytes(_imageBitmap);
        }


        public byte[] Negative(byte[] image)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }

            Color curColor;

            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    curColor = _imageBitmap.GetPixel(iX, iY);
                    curColor = Color.FromArgb(curColor.A, 255 - curColor.R, 255 - curColor.G, 255 - curColor.B);

                    _imageBitmap.SetPixel(iX, iY, curColor);
                }
            }

            return Conventor.BitmapToBytes(_imageBitmap);
        }


        public byte[] Noise(byte[] image, double probability)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }

            Color curColor;
            var rand = new Random();

            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    curColor = _imageBitmap.GetPixel(iX, iY);

                    if (rand.NextDouble() < probability)
                    {
                        int bright = (rand.NextDouble() > 0.5) ? 0 : 255;
                        curColor = Color.FromArgb(curColor.A, bright, bright, bright);
                    }

                    _imageBitmap.SetPixel(iX, iY, curColor);
                }
            }

            return Conventor.BitmapToBytes(_imageBitmap);
        }


        public byte[] LinearSmoothing(byte[] image)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }
            Bitmap _processedBitmap = new(_imageBitmap.Width, _imageBitmap.Height);
            Color curColor;

            for (int iY = 0; iY < _imageBitmap.Height; iY++)
            {
                for (int iX = 0; iX < _imageBitmap.Width; iX++)
                {
                    var aperture = GetApertureFromCenter(_imageBitmap, iX, iY);
                    curColor = PixelsHandler.GetAvgColor(aperture);
                    _imageBitmap.SetPixel(iX, iY, curColor);
                }
            }

            return Conventor.BitmapToBytes(_imageBitmap);
        }


        public byte[] MedianSmoothing(byte[] image)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }
            Bitmap _processedBitmap = new(_imageBitmap.Width, _imageBitmap.Height);
            Color curColor;

            for (int iY = 0; iY < _imageBitmap.Height; iY++)
            {
                for (int iX = 0; iX < _imageBitmap.Width; iX++)
                {
                    var aperture = GetApertureFromCenter(_imageBitmap, iX, iY);
                    curColor = PixelsHandler.GetMedianPixel(aperture);
                    _processedBitmap.SetPixel(iX, iY, curColor);
                }
            }

            return Conventor.BitmapToBytes(_processedBitmap);
        }


        public static List<Color> GetPixelsAroundClock(Bitmap image, int i, int j)
        {
            List<Color> pixels = new();
            int[,] deltas = new int[,] {
                { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0,  1 },
                { 1,  1 }, { 1,  0 }, { 1,  -1 }, { 0,  -1 }
            };

            for (int d = 0; d < 8; d++)
            {
                int y = i + deltas[d, 0];
                int x = j + deltas[d, 1];

                if ((x >= 0) && (y >= 0) && (x < image.Width) && (y < image.Height) && !(y == i && x == j))
                {
                    pixels.Add(image.GetPixel(x, y));
                }
            }

            return pixels;
        }

        public static List<Color> GetApertureFromCenter(Bitmap image, int sx, int sy)
        {
            List<Color> pixels = new();
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    int x = sx + dx;
                    int y = sy + dy;
                    if ((x >= 0) && (y >= 0) && (x < image.Width) && (y < image.Height))
                    {
                        pixels.Add(image.GetPixel(x, y));
                    }
                }
            }
            return pixels;
        }

        public static List<Color> GetApertureFromWNCorner(Bitmap image, int sx, int sy)
        {
            List<Color> pixels = new();
            for (int dy = 0; dy <= 2; dy++)
            {
                for (int dx = 0; dx <= 2; dx++)
                {
                    int x = sx + dx;
                    int y = sy + dy;
                    if ((x < image.Width) && (y < image.Height))
                    {
                        pixels.Add(image.GetPixel(x, y));
                    }
                }
            }
            return pixels;
        }


        public byte[] KirschsMethod(byte[] image)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }

            Bitmap _processedBitmap = new(_imageBitmap.Width, _imageBitmap.Height);
            for (int iY = 1; iY < _imageBitmap.Height - 1; iY++)
            {
                for (int iX = 1; iX < _imageBitmap.Width - 1; iX++)
                {
                    var newPixel = KirschsF(_imageBitmap, iY, iX);
                    _processedBitmap.SetPixel(iX, iY, newPixel);
                }
            }

            return Conventor.BitmapToBytes(_processedBitmap);
        }

        public byte[] LaplasMethod(byte[] image)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }

            Bitmap _processedBitmap = new(_imageBitmap.Width, _imageBitmap.Height);

            for (int iY = 1; iY < _imageBitmap.Height - 1; iY++)
            {
                for (int iX = 1; iX < _imageBitmap.Width - 1; iX++)
                {
                    var newPixel = LaplasF(_imageBitmap, iX, iY);
                    _processedBitmap.SetPixel(iX, iY, newPixel);
                }
            }

            return Conventor.BitmapToBytes(_processedBitmap);
        }

        public byte[] RobertsMethod(byte[] image)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }

            for (int iY = 0; iY < _imageBitmap.Height - 1; iY++)
            {
                for (int iX = 0; iX < _imageBitmap.Width - 1; iX++)
                {

                    int Fij = PixelsHandler.GetPixelBrightness(_imageBitmap.GetPixel(iX, iY));
                    int Fi1j1 = PixelsHandler.GetPixelBrightness(_imageBitmap.GetPixel(iX + 1, iY + 1));
                    int Fi1j = PixelsHandler.GetPixelBrightness(_imageBitmap.GetPixel(iX + 1, iY));
                    int Fij1 = PixelsHandler.GetPixelBrightness(_imageBitmap.GetPixel(iX, iY + 1));

                    int ret = (int)Math.Sqrt(
                        ((Fij - Fi1j1) * (Fij - Fi1j1)) +
                        ((Fij1 - Fi1j) * (Fij1 - Fi1j))
                        );
                    if (ret > 255) { ret = 255; }
                    else if (ret < 0) { ret = 0; }
                    var newPixel = Color.FromArgb(ret, ret, ret, ret);
                    _imageBitmap.SetPixel(iX, iY, newPixel);
                }
            }

            return Conventor.BitmapToBytes(_imageBitmap);
        }


        public byte[] SobelMethod(byte[] image)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }

            Bitmap _processedBitmap = new(_imageBitmap.Width, _imageBitmap.Height);

            for (int iY = 1; iY < _imageBitmap.Height - 1; iY++)
            {
                for (int iX = 1; iX < _imageBitmap.Width - 1; iX++)
                {
                    var newPixel = SobelF(_imageBitmap, iX, iY);
                    _processedBitmap.SetPixel(iX, iY, newPixel);
                }
            }

            return Conventor.BitmapToBytes(_processedBitmap);
        }

        public byte[] WallisMethod(byte[] image)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }

            Bitmap _processedBitmap = new(_imageBitmap.Width, _imageBitmap.Height);
            for (int iY = 1; iY < _imageBitmap.Height - 1; iY++)
            {
                for (int iX = 1; iX < _imageBitmap.Width - 1; iX++)
                {
                    var newPixel = WallisF(_imageBitmap, iX, iY);
                    _processedBitmap.SetPixel(iX, iY, newPixel);
                }
            }

            return Conventor.BitmapToBytes(_processedBitmap);
        }


        public byte[] StaticMethod(byte[] image)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }

            Bitmap _processedBitmap = new(_imageBitmap.Width, _imageBitmap.Height);
            for (int iY = 0; iY < _imageBitmap.Height; iY += 1)
            {
                for (int iX = 0; iX < _imageBitmap.Width; iX += 1)
                {
                    var aperture = GetApertureFromWNCorner(_imageBitmap, iX, iY);

                    double apertAvgBrightness = 0;
                    aperture.ForEach(c => apertAvgBrightness += PixelsHandler.GetPixelBrightness(c) / aperture.Count);

                    double sum = 0;
                    aperture.ForEach(c => sum += Math.Pow(PixelsHandler.GetPixelBrightness(c) - apertAvgBrightness, 2.0));

                    double apertAvgBrightnessDif = Math.Sqrt(sum / aperture.Count);

                    List<Color> newAperture = new();
                    foreach (Color pixel in aperture)
                    {
                        int bright = (int)Math.Round(PixelsHandler.GetPixelBrightness(pixel) * apertAvgBrightnessDif, MidpointRounding.AwayFromZero);
                        bright -= 1200;
                        if (bright > 255) { bright = 255; }
                        else if (bright < 0) { bright = 0; }
                        var newPixel = Color.FromArgb(pixel.A, bright, bright, bright);
                        newAperture.Add(newPixel);
                    }

                    int count = 0;
                    for (int y = 0; (y < 3) && ((iY + y) < _imageBitmap.Height); y++)
                    {
                        for (int x = 0; (x < 3) && ((iX + x) < _imageBitmap.Width); x++)
                        {
                            _processedBitmap.SetPixel(iX + x, iY + y, newAperture[count]);
                            count++;
                        }
                    }
                }
            }

            return Conventor.BitmapToBytes(_processedBitmap);
        }


        public static Color KirschsF(Bitmap image, int y, int x)
        {
            var A = GetPixelsAroundClock(image, y, x);
            List<int> S = new();
            List<int> T = new();
            List<int> F = new();

            for (int i = 0; i < A.Count; i++)
            {
                S.Add(PixelsHandler.GetPixelBrightness(A[i]) + PixelsHandler.GetPixelBrightness(A[(i + 1) % 8]) + PixelsHandler.GetPixelBrightness(A[(i + 2) % 8]));
                T.Add(PixelsHandler.GetPixelBrightness(A[(i + 3) % 8]) + PixelsHandler.GetPixelBrightness(A[(i + 4) % 8]) + PixelsHandler.GetPixelBrightness(A[(i + 5) % 8]) + PixelsHandler.GetPixelBrightness(A[(i + 6) % 8]) + PixelsHandler.GetPixelBrightness(A[(i + 7) % 8]));
                F.Add(Math.Abs((5 * S[i]) - (3 * T[i])));
            }

            int ret = F.Max() - 50;

            if (ret > 255) { ret = 255; }
            else if (ret < 0) { ret = 0; }

            return Color.FromArgb(image.GetPixel(x, y).A, ret, ret, ret);
        }

        public static Color LaplasF(Bitmap image, int x, int y)
        {
            int[] laplasMatrix = new int[] {
                 -1, -2, -1,
                 -2, 12, -2,
                 -1, -2, -1
            };

            var aperture = GetApertureFromCenter(image, x, y);
            var brightnesses = new int[9];

            for (int i = 0; i < aperture.Count; i++)
            {
                brightnesses[i] = PixelsHandler.GetPixelBrightness(aperture[i]) * laplasMatrix[i];
            }

            int ret = brightnesses.Sum();

            if (ret > 255) { ret = 255; }
            else if (ret < 0) { ret = 0; }

            return Color.FromArgb(image.GetPixel(x, y).A, ret, ret, ret);
        }

        private static Color SobelF(Bitmap image, int x, int y)
        {
            var aperture = GetApertureFromCenter(image, x, y);

            int X = (PixelsHandler.GetPixelBrightness(aperture[0]) + 2 * PixelsHandler.GetPixelBrightness(aperture[1]) + PixelsHandler.GetPixelBrightness(aperture[2]))
                - (PixelsHandler.GetPixelBrightness(aperture[6]) + 2 * PixelsHandler.GetPixelBrightness(aperture[7]) + PixelsHandler.GetPixelBrightness(aperture[8]));
            int Y = (PixelsHandler.GetPixelBrightness(aperture[0]) + 2 * PixelsHandler.GetPixelBrightness(aperture[3]) + PixelsHandler.GetPixelBrightness(aperture[6]))
                - (PixelsHandler.GetPixelBrightness(aperture[2]) + 2 * PixelsHandler.GetPixelBrightness(aperture[5]) + PixelsHandler.GetPixelBrightness(aperture[8]));

            int ret = (int)Math.Sqrt((X * X) + (Y * Y));

            if (ret > 255) { ret = 255; }
            else if (ret < 0) { ret = 0; }

            return Color.FromArgb(image.GetPixel(x, y).A, ret, ret, ret);
        }

        private static Color WallisF(Bitmap image, int x, int y)
        {
            var aperture = GetApertureFromCenter(image, x, y);
            var curPixel = image.GetPixel(x, y);
            double F = PixelsHandler.GetPixelBrightness(curPixel);
            double A1 = PixelsHandler.GetPixelBrightness(aperture[1]);
            double A3 = PixelsHandler.GetPixelBrightness(aperture[3]);
            double A5 = PixelsHandler.GetPixelBrightness(aperture[5]);
            double A7 = PixelsHandler.GetPixelBrightness(aperture[7]);
            A1 = (A1 == 0) ? 1 : A1;
            A3 = (A3 == 0) ? 1 : A3;
            A5 = (A5 == 0) ? 1 : A5;
            A7 = (A7 == 0) ? 1 : A7;

            double ln = Math.Log(((F / A1) + (F / A3) + (F / A5) + (F / A7)), Math.E);
            int ret = (int)Math.Round((1000 * ln / 4.0), MidpointRounding.AwayFromZero);
            ret -= 250;
            if (ret > 255) { ret = 255; }
            else if (ret < 0) { ret = 0; }

            return Color.FromArgb(image.GetPixel(x, y).A, ret, ret, ret);
        }

    }
}