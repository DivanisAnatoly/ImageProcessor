using System;

namespace ImageProcessor.Processor.Contracts
{
    public class BrightnessDistribution
    {
        public int[] Distribution { get; private set; }
        public int[] RedChanalDistribution { get; private set; }
        public int[] GreenChanalDistribution { get; private set; }
        public int[] BlueChanalDistribution { get; private set; }
       
        public int PixelNum { get; }

        public int AvgBrightness { get; private set; }
        public int RAvgBrightness { get; private set; }
        public int GAvgBrightness { get; private set; }
        public int BAvgBrightness { get; private set; }
        public int AvgBrightnessDiffer { get; private set; }


        public BrightnessDistribution(int[] distribution, int[] redChanalDistrib,
            int[] greenChanalDistrib, int[] blueChanalDistrib, int pixelNum)
        {
            Distribution = distribution;
            RedChanalDistribution = redChanalDistrib;
            GreenChanalDistribution = greenChanalDistrib;
            BlueChanalDistribution = blueChanalDistrib;
            PixelNum = pixelNum;

            AvgBrightness = CalculateAvgChanalColors(distribution);
            RAvgBrightness = CalculateAvgChanalColors(redChanalDistrib);
            GAvgBrightness = CalculateAvgChanalColors(greenChanalDistrib);
            BAvgBrightness = CalculateAvgChanalColors(blueChanalDistrib);

            AvgBrightnessDiffer = CalculateAvgBrightnessDiffer(distribution, AvgBrightness);
        }

        private int CalculateAvgChanalColors(int[] chanalDistrib)
        {
            long sum = 0;

            for (int i = 0; i < chanalDistrib.Length; i++)
            {
                sum += i * chanalDistrib[i];
            }

            return (int)sum / PixelNum;
        }
        
        private int CalculateAvgBrightnessDiffer(int[] chanalDistrib, int chanalAvgBrightness)
        {
            double sum = 0;

            for (int i = 0; i < chanalDistrib.Length; i++)
            {
                sum += chanalDistrib[i] * Math.Pow(i-chanalAvgBrightness,2.0) / PixelNum;
            }

            return (int)Math.Sqrt(sum);
        }
    }
}
