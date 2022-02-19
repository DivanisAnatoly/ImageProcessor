using System.Windows;
using System.Windows.Media;

namespace ImageProcessor.UI.PopupForms
{
    /// <summary>
    /// Логика взаимодействия для BinarizeSettingsPopUp.xaml
    /// </summary>
    public partial class BinarizeSettingsPopUp : Window
    {
        public Color? MainColor { get; private set; }
        public Color? BackgroundColor { get; private set; }
        public int BinarizationLevel { get; private set; }

        public BinarizeSettingsPopUp()
        {
            InitializeComponent();

            mainColorPicker.SelectedColor = Colors.White;
            mainColorPicker.ShowDropDownButton = true;

            backgroundColorPicker.SelectedColor = Colors.Black;
            backgroundColorPicker.ShowDropDownButton = true;
        }

        private void Accept_Button_Click(object sender, RoutedEventArgs e)
        {
            MainColor = mainColorPicker.SelectedColor;
            BackgroundColor = backgroundColorPicker.SelectedColor;
            BinarizationLevel = (int)binarizationLevel.Value;
            DialogResult = true;
            Close();
        }
    }
}
