using System.Windows;

namespace TravelSystem.Client.Views
{
    public partial class DataFormatSelectionWindow : Window
    {
        public string SelectedFormat { get; private set; } = "json";

        public DataFormatSelectionWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (XmlRadioButton.IsChecked == true)
                SelectedFormat = "xml";
            else if (JsonRadioButton.IsChecked == true)
                SelectedFormat = "json";
            else if (CsvRadioButton.IsChecked == true)
                SelectedFormat = "csv";

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}