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
using System.Windows.Shapes;
using static GeoApify_Geocode_Finder.MainWindow;

namespace GeoApify_Geocode_Finder
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup : Window
    {
        public Setup()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string key = txtSetup.Text.Trim();
            if (!string.IsNullOrEmpty(key))
            {
                File.WriteAllText("setup.ini", key);
                MainWindow.API_KEY = key;
                MessageBox.Show("API key saved in 'setup.ini'", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid API key.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
