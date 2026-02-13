using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Windows;
using Verkehrs_Informationen.Views;
using Verkehrs_Informationen.Models;
using Verkehrs_Informationen.APIs;
using Verkehrs_Informationen.Models.ViewModels;
using System.Diagnostics;

namespace Verkehrs_Informationen
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AutobahnAPI autobahnAPI = new AutobahnAPI();
        private MainViewModel mainViewModel = new MainViewModel();

        public App()
        {
            Task.Run(async () => mainViewModel.Roads = await autobahnAPI.GetRoads()).Wait();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();
        }
    }
}
