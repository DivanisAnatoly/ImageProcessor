using ImageProcessor.Processor.Contracts;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ImageProcessor.UI.ViewModels
{
    public class BrightnessProfileLineGraphViewModel
    {
        public SeriesCollection ChartSeriesCollection { get; set; } = new();
        public LineSeries RedSeries { get; private set; }
        public LineSeries GreenSeries { get; private set; }
        public LineSeries BlueSeries { get; private set; }


        public BrightnessProfileLineGraphViewModel(BrightnessProfileData profileData)
        {

            RedSeries = new LineSeries
            {
                PointGeometrySize=0,
                Fill =  Brushes.Transparent,
                Values = new ChartValues<int>(profileData.RedChanalProfile),
                Title = "Red Profile",
                Stroke = Brushes.Red
            };

            GreenSeries = new LineSeries
            {
                PointGeometrySize = 0,
                Fill = Brushes.Transparent,
                Values = new ChartValues<int>(profileData.GreenChanalProfile),
                Title = "Green Profile",
                Stroke = Brushes.Green
            };

            BlueSeries = new LineSeries
            {
                PointGeometrySize = 0,
                Fill = Brushes.Transparent,
                Values = new ChartValues<int>(profileData.BlueChanalProfile),
                Title = "Blue Profile",
                Stroke = Brushes.Blue
            };

            ChartSeriesCollection.Add(RedSeries);
            ChartSeriesCollection.Add(GreenSeries);
            ChartSeriesCollection.Add(BlueSeries);
        }
    }
}
