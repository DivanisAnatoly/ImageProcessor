using ImageProcessor.UI.Common;
using ImageProcessor.Processor.Filters;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageProcessor.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FilterHandler filterHandler = new();
        byte[] _originImage;
        byte[] _processedImage;

        public MainWindow()
        {
            InitializeComponent();
            this.filters.ItemsSource = Enum.GetValues(typeof(Filter)).Cast<Filter>();
        }

        private void BtnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                BitmapImage file = new BitmapImage(new Uri(op.FileName));
                originImage.Source = file;
                
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(file));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    _originImage = ms.ToArray();
                }

            }
        }

        private void Clean_Button_Click(object sender, RoutedEventArgs e)
        {
            originImage.Source = null;
        }

        private void Process_Button_Click(object sender, RoutedEventArgs e)
        {
            //ComboBoxItem selectedItem = (ComboBoxItem)filters.SelectedItem;
            //MessageBox.Show(selectedItem.Content.ToString());
            Filter filter = (Filter)filters.SelectedItem;

            switch (filter)
            {
                case Filter.Binarize :
                    processedImage = 
            }

        }


        /*
        ---------- Binarize ----------
        
        Color mainColor = Color.FromArgb(34,46,187,86);
        Color backgroundColor = Color.FromArgb(204,205,97,46);
        processedImage = filterHandler.Binarize(image,130, mainColor, backgroundColor);
        

        ---------- Grayscale ----------
        processedImage = filterHandler.Grayscale(image);

        ---------- Negative ----------
        processedImage = filterHandler.Negative(image);
        pictureBox1.Image = processedImage;
        */

    }
}
