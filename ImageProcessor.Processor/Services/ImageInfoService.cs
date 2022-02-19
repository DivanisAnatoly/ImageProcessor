using ImageProcessor.Processor.Contracts;
using ImageProcessor.Processor.Interfaces;
using ImageProcessor.Processor.Services.Support;
using ImageProcessor.Processor.Services.Support.MathModule;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ImageProcessor.Processor.Services
{
    public class ImageInfoService : IImageInfoService
    {
        private Bitmap _imageBitmap;

        public BrightnessDistribution GetBrightnessDistributionInfo(byte[] image)
        {
            Color curColor;
            int brightness;
            int[] distribution = new int[256];
            int[] redChanalDistribution = new int[256];
            int[] greenChanalDistribution = new int[256];
            int[] blueChanalDistribution = new int[256];

            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }

            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    curColor = _imageBitmap.GetPixel(iX, iY);
                    brightness = (int)(curColor.R * 0.3 + curColor.G * 0.59 + curColor.B * 0.11);

                    distribution[brightness] += 1;
                    redChanalDistribution[curColor.R] += 1;
                    greenChanalDistribution[curColor.G] += 1;
                    blueChanalDistribution[curColor.B] += 1;
                }
            }

            return new BrightnessDistribution(distribution, redChanalDistribution, greenChanalDistribution, blueChanalDistribution, (_imageBitmap.Width*_imageBitmap.Height));
        }


        public BrightnessProfileData GetBrightnessProfileInfo(byte[] image, double fromX, double fromY, double toX, double toY)
        {
            Point from = new((int)fromX, (int)fromY);
            Point to = new((int)toX, (int)toY);
            int maxX = Math.Max(from.X, to.X);
            int minX = Math.Min(from.X, to.X);
            int maxY = Math.Max(from.Y, to.Y);
            int minY = Math.Min(from.Y, to.Y);

            List<Point> profile = new();

            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }

            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                if (!(iX >= minX && iX <= maxX)) continue;
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    Point point = new(iX, iY);
                    if (iY >= minY && iY <= maxY && MathModule.DistanceToLine(from, to, point) < 0.5)
                    {
                        profile.Add(point);
                    }
                }
            }

            int i = 0;
            int count = profile.Count;
            int[] redProfile = new int[count];
            int[] greenProfile = new int[count];
            int[] blueProfile = new int[count];
            
            foreach (Point pixel in profile)
            {
                Color curColor = _imageBitmap.GetPixel(pixel.X, pixel.Y);
                
                redProfile[i] = curColor.R;
                greenProfile[i] = curColor.G;
                blueProfile[i] = curColor.B;
                i++;
            }

            return new BrightnessProfileData(redProfile, greenProfile, blueProfile);

        }

    }
}