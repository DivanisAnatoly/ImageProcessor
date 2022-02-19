using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ImageProcessor.UI.PopupForms
{
    /// <summary>
    /// Логика взаимодействия для BrightnessAndContrastPopUP.xaml
    /// </summary>
    public partial class BrightnessAndContrastPopUP : Window
    {
        public int BrightnessChange { get; private set; }
        public int ContrastChange { get; private set; }

        public BrightnessAndContrastPopUP()
        {
            InitializeComponent();
            VBrightnessSlider.Value = 0;
            VBrightnessSlider.Maximum = 255;
            VBrightnessSlider.Minimum = -255;
            VContrastSlider.Value = 0;
            VContrastSlider.Maximum = 100;
            VContrastSlider.Minimum = -100;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            BrightnessChange = (int)VBrightnessSlider.Value;
            ContrastChange = (int)VContrastSlider.Value;
            DialogResult = true;
            Close();

        }
    }
}
