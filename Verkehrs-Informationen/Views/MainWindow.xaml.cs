using System.Windows;
using System.Windows.Input;
using Verkehrs_Informationen.Models;
using Verkehrs_Informationen.Models.ViewModels;

namespace Verkehrs_Informationen.Views
{
    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

       
        private void WarningList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            if (viewModel?.Warnings != null && sender is System.Windows.Controls.ListBox listBox)
            {
                if (listBox.SelectedItem is WarningItem selectedItem)
                {
                    viewModel.ShowDetailCommand.Execute(selectedItem);
                }
            }
        }

       
        private void ClosedRoads_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            if (viewModel?.ClosedRoads != null && sender is System.Windows.Controls.DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem is WarningItem selectedItem)
                {
                    viewModel.ShowDetailCommand.Execute(selectedItem);
                }
            }
        }

       
        private void RoadWorks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            if (viewModel?.RoadWorks != null && sender is System.Windows.Controls.DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem is WarningItem selectedItem)
                {
                    viewModel.ShowDetailCommand.Execute(selectedItem);
                }
            }
        }

      
        private void DialogOverlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            var viewModel = DataContext as MainViewModel;
            if (e.Source == sender)
            {
                viewModel?.CloseDialogCommand.Execute(null);
            }
        }

    
        private void Dialog_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          
            e.Handled = true;
        }
    }
}