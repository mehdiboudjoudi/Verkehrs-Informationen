using System.Threading.Tasks;
using System.Windows;
using Verkehrs_Informationen.Views;
using Verkehrs_Informationen.APIs;
using Verkehrs_Informationen.Models.ViewModels;

namespace Verkehrs_Informationen
{
    public partial class App : Application
    {
        private AutobahnAPI autobahnAPI = new AutobahnAPI();
        private MainViewModel mainViewModel = new MainViewModel();

        public App()
        {
            // Lade die Liste aller Autobahnen im Hintergrund
            Task.Run(async () => mainViewModel.Roads = await autobahnAPI.GetRoads()).Wait();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Lade die zwischengespeicherten Daten aus der DB in das ViewModel!
            mainViewModel.LoadDataFromDatabase();

            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();
        }
    }
}