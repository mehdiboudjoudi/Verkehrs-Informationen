using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Verkehrs_Informationen.APIs;
using Verkehrs_Informationen.Models;
using Verkehrs_Informationen.Database.Models; // WICHTIG für die DB-Entities

namespace Verkehrs_Informationen.Models.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly AutobahnAPI _autobahnAPI = new();

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
                // API-Call starten, sobald der Nutzer eine Straße auswählt
                LoadDataCommand.Execute(null);
            }
        }

        private ObservableCollection<WarningItem> _warnings = new();
        public ObservableCollection<WarningItem> Warnings
        {
            get => _warnings;
            set { _warnings = value; OnPropertyChanged(); }
        }

        private ObservableCollection<WarningItem> _closedRoads = new();
        public ObservableCollection<WarningItem> ClosedRoads
        {
            get => _closedRoads;
            set { _closedRoads = value; OnPropertyChanged(); }
        }

        private ObservableCollection<WarningItem> _roadWorks = new();
        public ObservableCollection<WarningItem> RoadWorks
        {
            get => _roadWorks;
            set { _roadWorks = value; OnPropertyChanged(); }
        }

        private string _detailTitle = "";
        public string DetailTitle
        {
            get => _detailTitle;
            set { _detailTitle = value; OnPropertyChanged(); }
        }

        private string _detailDescription = "";
        public string DetailDescription
        {
            get => _detailDescription;
            set { _detailDescription = value; OnPropertyChanged(); }
        }

        private bool _isDialogVisible;
        public bool IsDialogVisible
        {
            get => _isDialogVisible;
            set { _isDialogVisible = value; OnPropertyChanged(); }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand ShowDetailCommand { get; }
        public ICommand CloseDialogCommand { get; }

        public MainViewModel()
        {
            LoadDataCommand = new RelayCommand(async _ => await LoadData());
            ShowDetailCommand = new RelayCommand<WarningItem>(ShowDetail);
            CloseDialogCommand = new RelayCommand(_ => IsDialogVisible = false);
        }

        /// <summary>
        /// Läd die zuletzt gespeicherten Daten aus der lokalen Datenbank beim Programmstart.
        /// </summary>
        public void LoadDataFromDatabase()
        {
            using var db = new MyDatabaseContext();

            // 1. Die zuletzt gewählte Straße laden
            var savedRoad = db.Road.FirstOrDefault();
            if (savedRoad != null)
            {
                _selectedRoad = savedRoad.Road;
                OnPropertyChanged(nameof(SelectedRoad));
            }

            // 2. Warnungen laden: Erst mit .ToList() in den Speicher holen, dann mappen!
            var savedWarnings = db.Warning.ToList();
            Warnings = new ObservableCollection<WarningItem>(savedWarnings.Select(w => new WarningItem
            {
                Title = w.Title,
                Subtitle = w.Subtitle,
                IsBlocked = w.IsBlocked,
                DescriptionList = string.IsNullOrEmpty(w.FullDescription) ? new List<string>() : new List<string>(w.FullDescription.Split('\n'))
            }));

            // 3. Sperrungen laden: Erst mit .ToList() in den Speicher holen, dann mappen!
            var savedClosures = db.Closure.ToList();
            ClosedRoads = new ObservableCollection<WarningItem>(savedClosures.Select(c => new WarningItem
            {
                Title = c.Title,
                Subtitle = c.Subtitle,
                IsBlocked = c.IsBlocked,
                DescriptionList = string.IsNullOrEmpty(c.FullDescription) ? new List<string>() : new List<string>(c.FullDescription.Split('\n'))
            }));

            // 4. Baustellen laden: Erst mit .ToList() in den Speicher holen, dann mappen!
            var savedRoadWorks = db.RoadWork.ToList();
            RoadWorks = new ObservableCollection<WarningItem>(savedRoadWorks.Select(r => new WarningItem
            {
                Title = r.Title,
                Subtitle = r.Subtitle,
                IsBlocked = r.IsBlocked,
                DescriptionList = string.IsNullOrEmpty(r.FullDescription) ? new List<string>() : new List<string>(r.FullDescription.Split('\n'))
            }));
        }

        private async Task LoadData()
        {
            if (string.IsNullOrEmpty(_selectedRoad))
                return;

            // 1. Frische Daten aus der API laden
            await LoadWarnings();
            await LoadClosedRoads();
            await LoadRoadWorks();

            // 2. Sobald alles geladen ist, die Datenbank wipen und neu füllen!
            await SaveDataToDatabaseAsync();
        }

        /// <summary>
        /// Löscht die komplette Datenbank und speichert die frisch aus der API geladenen Daten.
        /// </summary>
        private async Task SaveDataToDatabaseAsync()
        {
            using var db = new MyDatabaseContext();

            // 1. ALLES aus den Tabellen löschen (Wipe)
            db.Warning.RemoveRange(db.Warning);
            db.Closure.RemoveRange(db.Closure);
            db.RoadWork.RemoveRange(db.RoadWork);
            db.Road.RemoveRange(db.Road);

            // 2. Die neue RoadId einfügen
            db.Road.Add(new RoadIdEntity { Road = _selectedRoad });

            // 3. Die neuen Daten einfügen (DTO -> Entity Mapping)
            db.Warning.AddRange(Warnings.Select(w => new WarningEntity
            {
                Title = w.Title,
                Subtitle = w.Subtitle,
                IsBlocked = w.IsBlocked,
                FullDescription = w.FullDescription
            }));

            db.Closure.AddRange(ClosedRoads.Select(c => new ClosureEntity
            {
                Title = c.Title,
                Subtitle = c.Subtitle,
                IsBlocked = c.IsBlocked,
                FullDescription = c.FullDescription
            }));

            db.RoadWork.AddRange(RoadWorks.Select(r => new RoadWorkEntity
            {
                Title = r.Title,
                Subtitle = r.Subtitle,
                IsBlocked = r.IsBlocked,
                FullDescription = r.FullDescription
            }));

            // 4. In der Datenbank speichern
            await db.SaveChangesAsync();
        }

        private async Task LoadWarnings()
        {
            var warnings = await _autobahnAPI.GetWarnings(_selectedRoad);
            Warnings.Clear();
            if (warnings != null)
                foreach (var warning in warnings) Warnings.Add(warning);
        }

        private async Task LoadClosedRoads()
        {
            var closedRoads = await _autobahnAPI.GetClosedRoads(_selectedRoad);
            ClosedRoads.Clear();
            if (closedRoads != null)
                foreach (var closedRoad in closedRoads) ClosedRoads.Add(closedRoad);
        }

        private async Task LoadRoadWorks()
        {
            var roadWorks = await _autobahnAPI.GetRoadWorks(_selectedRoad);
            RoadWorks.Clear();
            if (roadWorks != null)
                foreach (var roadWork in roadWorks) RoadWorks.Add(roadWork);
        }

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