using ImageProcessor.Processor.Interfaces;
using ImageProcessor.Processor.Services;
using ImageProcessor.UI.Common;
using ImageProcessor.UI.Image;
using ImageProcessor.UI.PopupForms;
using ImageProcessor.UI.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageProcessor.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Output formats
        private const string ImgHeightOutputFormat = "Height: {0} px";
        private const string ImgWidthOutputFormat = "Width: {0} px";
        private const string ImgExtensionOutputFormat = "Extension: {0}";
        private const string ImgMouseXOutputFormat = "X: {0:0.##}";
        private const string ImgMouseYOutputFormat = "Y: {0:0.##}";

        //Services
        private readonly IImageFiltersService _imageFiltersService;
        private readonly IImageAreasService _imageAreasService;

        //Common objects
        private ImageProperties _properties;

        //Controllers
        private byte[] _originImage;
        private byte[] _processedImage;
        private byte[] _lastProcessedImage;

        //Windows
        private readonly ImageDiagramsWindow _imageDiagramsWindow;


        public MainWindow()
        {
            InitializeComponent();
            this.filters.ItemsSource = Enum.GetValues(typeof(Filter)).Cast<Filter>();

            _imageFiltersService = new ImageFiltersService();
            _imageAreasService = new ImageAreasService();

            _imageDiagramsWindow = new();
        }


        private void BtnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new()
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == true)
            {
                BitmapImage file = new(new Uri(op.FileName));
                originImage.Source = file;

                JpegBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(file));
                using MemoryStream ms = new();
                encoder.Save(ms);

                _originImage = ms.ToArray();

                processedImage.Source = BytesToImageSource(_originImage);
                _processedImage = ms.ToArray();

                _properties = new(file);
                imgPropsHeight.Text = string.Format(ImgHeightOutputFormat, _properties.Height);
                imgPropsWidth.Text = string.Format(ImgWidthOutputFormat, _properties.Width);
                imgPropsExtension.Text = string.Format(ImgExtensionOutputFormat, _properties.Extension);

                _imageDiagramsWindow.InitWindowContent(_processedImage);
                _imageDiagramsWindow.ClearBrightnessProfile();
            }
        }

        private void Clean_Button_Click(object sender, RoutedEventArgs e)
        {
            originImage.Source = null;
            _imageDiagramsWindow.ClearBrightnessProfile();
        }

        private void Process_Button_Click(object sender, RoutedEventArgs e)
        {
            object selectedItem = filters.SelectedItem;
            if (selectedItem == null) { return; }

            Filter filter = (Filter)selectedItem;

            switch (filter)
            {
                case Filter.Binarize:
                    BinarizeSettingsPopUp binarizeSettings = new();
                    if (binarizeSettings.ShowDialog() == true)
                    {
                        Color? mc = binarizeSettings.MainColor;
                        Color? bc = binarizeSettings.BackgroundColor;
                        System.Drawing.Color mainColor = System.Drawing.Color.FromArgb(mc.Value.A, mc.Value.R, mc.Value.G, mc.Value.B);
                        System.Drawing.Color backgroundColor = System.Drawing.Color.FromArgb(bc.Value.A, bc.Value.R, bc.Value.G, bc.Value.B);
                        int binarizeLevel = binarizeSettings.BinarizationLevel;
                        _processedImage = _imageFiltersService.Binarize(_originImage, binarizeLevel, mainColor, backgroundColor);
                        processedImage.Source = BytesToImageSource(_processedImage);
                    }
                    break;
                case Filter.Grayscale:
                    _processedImage = _imageFiltersService.Grayscale(_originImage);
                    processedImage.Source = BytesToImageSource(_processedImage);
                    break;
                case Filter.Negative:
                    _processedImage = _imageFiltersService.Negative(_originImage);
                    processedImage.Source = BytesToImageSource(_processedImage);
                    break;
                case Filter.Brightness_Contrast:
                    BrightnessAndContrastPopUP brightnessContrastPopUP = new();
                    if (brightnessContrastPopUP.ShowDialog() == true)
                    {
                        int brightnessChange = brightnessContrastPopUP.BrightnessChange;
                        int contrastChange = brightnessContrastPopUP.ContrastChange;
                        _processedImage = _imageFiltersService.ChangeBrightness(_originImage, brightnessChange);
                        _processedImage = _imageFiltersService.ChangeContrast(_processedImage, contrastChange);
                        processedImage.Source = BytesToImageSource(_processedImage);
                    }
                    else
                    {
                        return;
                    }
                    break;
                case Filter.Noise:
                    NoisePopUp noisePopUP = new();
                    if (noisePopUP.ShowDialog() == true)
                    {
                        double noiseLevel = noisePopUP.NoiseLevel;
                        _processedImage = _imageFiltersService.Noise(_originImage, noiseLevel);
                        processedImage.Source = BytesToImageSource(_processedImage);
                    }
                    break;
                case Filter.LinearSmoothing:
                    _processedImage = _imageFiltersService.LinearSmoothing(_processedImage);
                    processedImage.Source = BytesToImageSource(_processedImage);
                    break;
                case Filter.MedianSmoothing:
                    _processedImage = _imageFiltersService.MedianSmoothing(_processedImage);
                    processedImage.Source = BytesToImageSource(_processedImage);
                    break;
                case Filter.KirschsMethod:
                    _processedImage = _imageFiltersService.KirschsMethod(_processedImage);
                    processedImage.Source = BytesToImageSource(_processedImage);
                    break;
                case Filter.LaplasMethod:
                    _processedImage = _imageFiltersService.LaplasMethod(_processedImage);
                    processedImage.Source = BytesToImageSource(_processedImage);
                    break;
                case Filter.RobertsMethod:
                    _processedImage = _imageFiltersService.RobertsMethod(_processedImage);
                    processedImage.Source = BytesToImageSource(_processedImage);
                    break;
                case Filter.SobelMethod:
                    _processedImage = _imageFiltersService.SobelMethod(_processedImage);
                    processedImage.Source = BytesToImageSource(_processedImage);
                    break;
                case Filter.WallisMethod:
                    _processedImage = _imageFiltersService.WallisMethod(_processedImage);
                    processedImage.Source = BytesToImageSource(_processedImage);
                    break;
                case Filter.StaticMethod:
                    _processedImage = _imageFiltersService.StaticMethod(_processedImage);
                    processedImage.Source = BytesToImageSource(_processedImage);
                    break;
                case Filter.Clustarization:
                    _processedImage = _imageAreasService.Clustarization(_processedImage);
                    processedImage.Source = BytesToImageSource(_processedImage);
                    break;
                default:
                    break;
            }
            _imageDiagramsWindow.UpdateImageInfo(_processedImage);
        }


        private static ImageSource BytesToImageSource(byte[] bArray)
        {
            return (ImageSource)new ImageSourceConverter().ConvertFrom(bArray);
        }


        public Point GetImageCoordsAt(MouseEventArgs e)
        {
            Point mouseOnImage;
            int x = -1;
            int y = -1;

            if (originImage != null && originImage.IsMouseOver)
            {
                mouseOnImage = e.GetPosition(originImage);
                x = (int)((_properties.Width / originImage.ActualWidth * mouseOnImage.X)) + 1;
                y = (int)(_properties.Height / originImage.ActualHeight * mouseOnImage.Y) + 1;
            }
            if (processedImage != null && processedImage.IsMouseOver)
            {
                mouseOnImage = e.GetPosition(processedImage);
                x = (int)((_properties.Width / processedImage.ActualWidth * mouseOnImage.X)) + 1;
                y = (int)(_properties.Height / processedImage.ActualHeight * mouseOnImage.Y) + 1;
            }
            mouseOnImage.X = x;
            mouseOnImage.Y = y;

            return mouseOnImage;
        }


        private void OriginImage_MouseMove(object sender, MouseEventArgs e)
        {
            Point mouseOnImage = GetImageCoordsAt(e);
            imgPropsMouseX.Text = string.Format(ImgMouseXOutputFormat, mouseOnImage.X);
            imgPropsMouseY.Text = string.Format(ImgMouseYOutputFormat, mouseOnImage.Y);
        }


        private void OriginImage_MouseLeave(object sender, MouseEventArgs e)
        {
            imgPropsMouseX.Text = string.Format(ImgMouseXOutputFormat, -1);
            imgPropsMouseY.Text = string.Format(ImgMouseYOutputFormat, -1);
        }


        private void BtnProcImgInfo_Click(object sender, RoutedEventArgs e)
        {
            _imageDiagramsWindow.Show();
        }


        private void ProcessedImage_MouseMove(object sender, MouseEventArgs e)
        {
            Point mouseOnImage = GetImageCoordsAt(e);
            imgPropsMouseX.Text = string.Format(ImgMouseXOutputFormat, mouseOnImage.X);
            imgPropsMouseY.Text = string.Format(ImgMouseYOutputFormat, mouseOnImage.Y);
        }


        private void ProcessedImage_MouseLeave(object sender, MouseEventArgs e)
        {
            imgPropsMouseX.Text = string.Format(ImgMouseXOutputFormat, -1);
            imgPropsMouseY.Text = string.Format(ImgMouseYOutputFormat, -1);
        }


        private DateTime downTime;
        private object downSender;
        private Point downPosition;
        readonly List<Point> linePoints = new();

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.downSender = sender;
                this.downTime = DateTime.Now;
                this.downPosition = GetImageCoordsAt(e);
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point mouseOnImage = GetImageCoordsAt(e);
            if (e.LeftButton == MouseButtonState.Released &&
            sender == this.downSender && this.downPosition == mouseOnImage)
            {
                TimeSpan timeSinceDown = DateTime.Now - this.downTime;
                if (timeSinceDown.TotalMilliseconds < 500)
                {
                    object selectedItem = filters.SelectedItem;
                    if (selectedItem != null && (Filter)selectedItem == Filter.MagicWand) {
                        if(_lastProcessedImage==null) _lastProcessedImage = _processedImage;
                        _processedImage = _lastProcessedImage;
                        _processedImage = _imageAreasService.MagicWand(_processedImage, (int)mouseOnImage.X, (int)mouseOnImage.Y, 50);
                        processedImage.Source = BytesToImageSource(_processedImage);
                    }

                    linePoints.Add(mouseOnImage);

                    if (linePoints.Count == 2)
                    {
                        if (selectedItem != null && (Filter)selectedItem == Filter.Dekstra)
                        {
                            _processedImage = _imageAreasService.Dekstra(_processedImage, (int)linePoints.First().X, (int)linePoints.First().Y, (int)linePoints.Last().X, (int)linePoints.Last().Y);
                            processedImage.Source = BytesToImageSource(_processedImage);
                            linePoints.Clear();
                            return;
                        }

                        _imageDiagramsWindow.ShowBrightnessProfile(linePoints.First(), linePoints.Last());
                        linePoints.Clear();
                    }
                }
            }
        }
    }
}