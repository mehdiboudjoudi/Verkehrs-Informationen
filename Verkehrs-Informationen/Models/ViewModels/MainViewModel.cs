using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Verkehrs_Informationen.APIs;
using Verkehrs_Informationen.Models;

namespace Verkehrs_Informationen.Models.ViewModels
{
    public class MainViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly AutobahnAPI _autobahnAPI = new AutobahnAPI();

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

        private string _selectedRoad;
        public string SelectedRoad
        {
            get => _selectedRoad;
            set
            {
                _selectedRoad = value;
                OnPropertyChanged();
                LoadWarningsCommand.Execute(null);
            }
        }

        private ObservableCollection<WarningItem> _warnings = new ObservableCollection<WarningItem>();
        public ObservableCollection<WarningItem> Warnings
        {
            get => _warnings;
            set
            {
                _warnings = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand LoadWarningsCommand { get; }

        public MainViewModel()
        {
            LoadWarningsCommand = new RelayCommand(async _ => await LoadWarnings());
        }

        private async Task LoadWarnings()
        {
            if (string.IsNullOrEmpty(_selectedRoad))
                return;

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
    }
}
