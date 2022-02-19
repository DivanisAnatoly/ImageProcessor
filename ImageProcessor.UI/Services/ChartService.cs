using ImageProcessor.Processor.Contracts;
using ImageProcessor.Processor.Interfaces;
using ImageProcessor.UI.Common;
using ImageProcessor.UI.Contracts;

namespace ImageProcessor.UI.Services
{
    public class ChartService
    {
        private readonly IImageInfoService _imageInfoService;


        public ChartService(IImageInfoService imageInfoService)
        {
            _imageInfoService = imageInfoService;
        }


        public BrightnessHistogramData ImageBrightnessDistribution(byte[] image)
        {
            BrightnessHistogramData histogramData = new();

            BrightnessDistribution distributionInfo = _imageInfoService.GetBrightnessDistributionInfo(image);

            histogramData.Distrib = new(distributionInfo.Distribution);
            histogramData.RDistrib = new(distributionInfo.RedChanalDistribution, ColorChanal.Red);
            histogramData.GDistrib = new(distributionInfo.GreenChanalDistribution, ColorChanal.Green);
            histogramData.BDistrib = new(distributionInfo.BlueChanalDistribution, ColorChanal.Blue);

            return histogramData;
        }


        /*public BrightnessHistogramData ImageBrightnessDistribution(byte[] image)
        {
            BrightnessHistogramData histogramData = new();

            BrightnessDistribution distributionInfo = _imageInfoService.GetBrightnessDistributionInfo(image);

            histogramData.Distrib = new(distributionInfo.Distribution);
            histogramData.RDistrib = new(distributionInfo.RedChanalDistribution, ColorChanal.Red);
            histogramData.GDistrib = new(distributionInfo.GreenChanalDistribution, ColorChanal.Green);
            histogramData.BDistrib = new(distributionInfo.BlueChanalDistribution, ColorChanal.Blue);

            return histogramData;
        }*/

    }
}
