using ImageProcessor.UI.Common;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace ImageProcessor.UI.ViewModels
{
    public class ImageBrightnessHistogramViewModel
    {
        public SeriesCollection ChartSeriesCollection { get; set; } = new();
        public ColumnSeries ColumnSeries { get; private set; }


        public ImageBrightnessHistogramViewModel(int[] values, ColorChanal chanal = ColorChanal.Default)
        {

            ColumnSeries = new ColumnSeries
            {
                Fill = chanal switch
                {
                    ColorChanal.Red => Brushes.Red,
                    ColorChanal.Green => Brushes.Green,
                    ColorChanal.Blue => Brushes.Blue,
                    ColorChanal.Default => Brushes.White,
                    _ => Brushes.White
                },
                Values = new ChartValues<int>(values),
                ColumnPadding = 0,
                Title = "Count"
            };
            ChartSeriesCollection.Add(ColumnSeries);
        }

    }
}
