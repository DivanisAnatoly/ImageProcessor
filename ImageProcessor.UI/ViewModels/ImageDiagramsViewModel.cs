using ImageProcessor.Processor.Interfaces;
using ImageProcessor.Processor.Services;

namespace ImageProcessor.UI.ViewModels
{
    public class ImageDiagramsViewModel
    {
        public ImageBrightnessHistogramViewModel ImageInfoChartViewModel { get; set; }

        public ImageDiagramsViewModel()
        {
            IImageInfoService imageInfoService = new ImageInfoService();
            //ImageInfoChartViewModel = ImageBrightnessHistogramViewModel.LoadViewModel(imageInfoService);
        }
    }
}
