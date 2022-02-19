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
    /// Логика взаимодействия для NoisePopUp.xaml
    /// </summary>
    public partial class NoisePopUp : Window
    {
        public double NoiseLevel { get; private set; }


        public NoisePopUp()
        {
            InitializeComponent();
            VNoiseProbability.Background = Brushes.DarkGray;
            VNoiseProbability.Items.Add(0.1);
            VNoiseProbability.Items.Add(0.2);
            VNoiseProbability.Items.Add(0.3);
            VNoiseProbability.Items.Add(0.4);
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            NoiseLevel = (double)VNoiseProbability.SelectedItem;
            DialogResult = true;
            Close();
        }
    }
}
