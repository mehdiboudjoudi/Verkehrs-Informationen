using System.Windows;
using System.Windows.Input;
using Verkehrs_Informationen.Models;
using Verkehrs_Informationen.Models.ViewModels;

namespace Verkehrs_Informationen.Views
{
    /// <summary>
    /// Main application window that displays traffic information for roads.
    /// Handles user interactions such as double-clicking list items to view details.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// Sets the DataContext to a MainViewModel instance for data binding.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        /// <summary>
        /// Handles double-click events on the warnings list box.
        /// Displays detailed information for the selected warning.
        /// </summary>
        /// <param name="sender">The ListBox control that raised the event.</param>
        /// <param name="e">The mouse button event arguments.</param>
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

        /// <summary>
        /// Handles double-click events on the closed roads data grid.
        /// Displays detailed information for the selected closed road.
        /// </summary>
        /// <param name="sender">The DataGrid control that raised the event.</param>
        /// <param name="e">The mouse button event arguments.</param>
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

        /// <summary>
        /// Handles double-click events on the road works data grid.
        /// Displays detailed information for the selected road work.
        /// </summary>
        /// <param name="sender">The DataGrid control that raised the event.</param>
        /// <param name="e">The mouse button event arguments.</param>
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

        /// <summary>
        /// Handles mouse click events on the dialog overlay (dark background).
        /// Closes the detail dialog when the user clicks outside the dialog itself.
        /// </summary>
        /// <param name="sender">The overlay element that raised the event.</param>
        /// <param name="e">The mouse button event arguments.</param>
        private void DialogOverlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Close the dialog when clicking on the dark background overlay
            var viewModel = DataContext as MainViewModel;
            if (e.Source == sender)
            {
                viewModel?.CloseDialogCommand.Execute(null);
            }
        }

        /// <summary>
        /// Handles mouse click events on the detail dialog.
        /// Prevents the dialog from closing when the user clicks on the dialog itself.
        /// </summary>
        /// <param name="sender">The dialog element that raised the event.</param>
        /// <param name="e">The mouse button event arguments.</param>
        private void Dialog_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Prevent closing the dialog when clicking on the dialog content itself
            e.Handled = true;
        }
    }
}