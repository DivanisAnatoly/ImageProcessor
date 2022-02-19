using ImageProcessor.Processor.Contracts;
using ImageProcessor.Processor.Interfaces;
using ImageProcessor.Processor.Services;
using ImageProcessor.UI.Contracts;
using ImageProcessor.UI.Services;
using ImageProcessor.UI.ViewModels;
using System.Windows;

namespace ImageProcessor.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для ImageDiagramsWindow.xaml
    /// </summary>
    public partial class ImageDiagramsWindow : Window
    {
        readonly IImageInfoService imageInfoService = new ImageInfoService();
        private byte[] _image;

        public ImageBrightnessHistogramViewModel ImageBrightnessHistogramViewModel { get; set; }
        public ImageBrightnessHistogramViewModel RImgBrightHistogramVM { get; set; }
        public ImageBrightnessHistogramViewModel GImgBrightHistogramVM { get; set; }
        public ImageBrightnessHistogramViewModel BImgBrightHistogramVM { get; set; }
        public BrightnessProfileLineGraphViewModel BrightnessProfileLineGraphVM { get; set; }


        public ImageDiagramsWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void InitWindowContent(byte[] image)
        {
            UpdateImageInfo(image);
        }

        public void UpdateImageInfo(byte[] image)
        {
            _image = image;


            ChartService chartService = new(imageInfoService);

            BrightnessHistogramData brightnessHistogramData = chartService.ImageBrightnessDistribution(_image);
            ImageBrightnessHistogramViewModel = brightnessHistogramData.Distrib;
            RImgBrightHistogramVM = brightnessHistogramData.RDistrib;
            GImgBrightHistogramVM = brightnessHistogramData.GDistrib;
            BImgBrightHistogramVM = brightnessHistogramData.BDistrib;

            DataContext = null;
            DataContext = this;
        }

        public void ShowBrightnessProfile(Point from, Point to)
        {
            if (_image == null)
            {
                return;
            }
            BrightnessProfileData profileData = imageInfoService.GetBrightnessProfileInfo(_image, from.X, from.Y, to.X, to.Y);
            BrightnessProfileLineGraphVM = new(profileData);
            VProfileGraph.DataContext = null;
            VProfileGraph.DataContext = BrightnessProfileLineGraphVM;
        }

        public void ClearBrightnessProfile()
        {
            VProfileGraph.DataContext = null;
        }

    }
}