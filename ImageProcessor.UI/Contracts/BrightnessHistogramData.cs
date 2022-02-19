using ImageProcessor.UI.ViewModels;

namespace ImageProcessor.UI.Contracts
{
    public class BrightnessHistogramData
    {
        public ImageBrightnessHistogramViewModel Distrib { get; set; }
        public ImageBrightnessHistogramViewModel RDistrib { get; set; }
        public ImageBrightnessHistogramViewModel GDistrib { get; set; }
        public ImageBrightnessHistogramViewModel BDistrib { get; set; }

    }
}
