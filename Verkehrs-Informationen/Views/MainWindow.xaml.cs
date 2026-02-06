using System.Windows;
using Verkehrs_Informationen.APIs;

namespace Verkehrs_Informationen.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoadWarnings_Click(object sender, RoutedEventArgs e)
        {
            string roadId = RoadInput.Text;

            var service = new AutobahnAPI();

            var warnings = await service.GetWarnings(roadId);

            if (warnings != null)
            {
                WarningList.ItemsSource = warnings;
            }
            else
            {
                MessageBox.Show("Fehler beim Laden der Daten.");
            }
        }
    }
}