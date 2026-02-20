using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Verkehrs_Informationen.APIs;
using Verkehrs_Informationen.Models;

namespace Verkehrs_Informationen.Models.ViewModels
{
    /// <summary>
    /// Main view model that manages the application's data and user interactions.
    /// Handles loading and displaying road information, warnings, closures, and road works.
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        /// <summary>
        /// API client instance for retrieving Autobahn traffic data.
        /// </summary>
        private readonly AutobahnAPI _autobahnAPI = new();

        /// <summary>
        /// Gets or sets the collection of available roads.
        /// </summary>
        private Road _roads;
        public Road Roads
        {
            get => _roads;
            set
            {
                _roads = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the currently selected road identifier.
        /// When changed, automatically triggers data loading for the selected road.
        /// </summary>
        private string _selectedRoad;
        public string SelectedRoad
        {
            get => _selectedRoad;
            set
            {
                _selectedRoad = value;
                OnPropertyChanged();
                LoadDataCommand.Execute(null);
            }
        }

        /// <summary>
        /// Gets or sets the collection of warnings for the selected road.
        /// </summary>
        private ObservableCollection<WarningItem> _warnings = new();
        public ObservableCollection<WarningItem> Warnings
        {
            get => _warnings;
            set
            {
                _warnings = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of closed roads for the selected road.
        /// </summary>
        private ObservableCollection<WarningItem> _closedRoads = new();
        public ObservableCollection<WarningItem> ClosedRoads
        {
            get => _closedRoads;
            set
            {
                _closedRoads = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of road works for the selected road.
        /// </summary>
        private ObservableCollection<WarningItem> _roadWorks = new();
        public ObservableCollection<WarningItem> RoadWorks
        {
            get => _roadWorks;
            set
            {
                _roadWorks = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the title displayed in the detail dialog.
        /// </summary>
        private string _detailTitle = "";
        public string DetailTitle
        {
            get => _detailTitle;
            set
            {
                _detailTitle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the detailed description displayed in the detail dialog.
        /// </summary>
        private string _detailDescription = "";
        public string DetailDescription
        {
            get => _detailDescription;
            set
            {
                _detailDescription = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility state of the detail dialog.
        /// </summary>
        private bool _isDialogVisible;
        public bool IsDialogVisible
        {
            get => _isDialogVisible;
            set
            {
                _isDialogVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command to load data for the selected road.
        /// </summary>
        public ICommand LoadDataCommand { get; }

        /// <summary>
        /// Command to display detailed information about a warning item.
        /// </summary>
        public ICommand ShowDetailCommand { get; }

        /// <summary>
        /// Command to close the detail dialog.
        /// </summary>
        public ICommand CloseDialogCommand { get; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// Sets up commands for loading data and managing the detail dialog.
        /// </summary>
        public MainViewModel()
        {
            LoadDataCommand = new RelayCommand(async _ => await LoadData());
            ShowDetailCommand = new RelayCommand<WarningItem>(ShowDetail);
            CloseDialogCommand = new RelayCommand(_ => IsDialogVisible = false);
        }

        /// <summary>
        /// Loads all traffic data (warnings, closures, and road works) for the selected road.
        /// </summary>
        private async Task LoadData()
        {
            // Exit early if no road is selected
            if (string.IsNullOrEmpty(_selectedRoad))
                return;

            // Load all data concurrently
            await LoadWarnings();
            await LoadClosedRoads();
            await LoadRoadWorks();
        }

        /// <summary>
        /// Loads warnings for the selected road from the API.
        /// </summary>
        private async Task LoadWarnings()
        {
            var warnings = await _autobahnAPI.GetWarnings(_selectedRoad);
            Warnings.Clear();
            if (warnings != null)
            {
                foreach (var warning in warnings)
                {
                    Warnings.Add(warning);
                }
            }
        }

        /// <summary>
        /// Loads closed roads information for the selected road from the API.
        /// </summary>
        private async Task LoadClosedRoads()
        {
            var closedRoads = await _autobahnAPI.GetClosedRoads(_selectedRoad);
            ClosedRoads.Clear();
            if (closedRoads != null)
            {
                foreach (var closedRoad in closedRoads)
                {
                    ClosedRoads.Add(closedRoad);
                }
            }
        }

        /// <summary>
        /// Loads road works information for the selected road from the API.
        /// </summary>
        private async Task LoadRoadWorks()
        {
            var roadWorks = await _autobahnAPI.GetRoadWorks(_selectedRoad);
            RoadWorks.Clear();
            if (roadWorks != null)
            {
                foreach (var roadWork in roadWorks)
                {
                    RoadWorks.Add(roadWork);
                }
            }
        }

        /// <summary>
        /// Displays the detail dialog with information about the selected warning item.
        /// </summary>
        /// <param name="item">The warning item to display details for.</param>
        private void ShowDetail(WarningItem item)
        {
            if (item != null)
            {
                DetailTitle = item.Title;
                DetailDescription = item.FullDescription;
                IsDialogVisible = true;
            }
        }
    }
}
