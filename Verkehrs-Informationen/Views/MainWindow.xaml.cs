using System.Collections.Generic;
using System.Windows;
using Verkehrs_Informationen.APIs;
using Verkehrs_Informationen.Models.ViewModels;
using Verkehrs_Informationen.Models;

namespace Verkehrs_Informationen.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoadData_Click(object sender, RoutedEventArgs e)
        {
            var service = new AutobahnAPI();
            string road = RoadInput.Text.ToUpper().Trim();

            if (string.IsNullOrEmpty(road)) return;

            LoadingIndicator.Visibility = Visibility.Visible;
            MasterGrid.ItemsSource = null;                  
            MasterGrid.IsEnabled = false;               

            try
            {
                var warningsTask = service.GetWarnings(road);
                var closuresTask = service.GetClosures(road);

                await Task.WhenAll(warningsTask, closuresTask);

                var allItems = new List<WarningItem>();
                if (warningsTask.Result != null) allItems.AddRange(warningsTask.Result);
                if (closuresTask.Result != null) allItems.AddRange(closuresTask.Result);

                MasterGrid.ItemsSource = allItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden: {ex.Message}");
            }
            finally
            {
                LoadingIndicator.Visibility = Visibility.Collapsed;
                MasterGrid.IsEnabled = true;
            }
        }

        private void MasterGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MasterGrid.SelectedItem is WarningItem selected)
            {
                DetailTitle.Text = selected.Title;
                DetailDesc.Text = selected.FullDescription;
                DetailDialog.Visibility = Visibility.Visible;
            }
        }

        private void CloseDialog_Click(object sender, RoutedEventArgs e)
        {
            DetailDialog.Visibility = Visibility.Collapsed;
        }
    }
}