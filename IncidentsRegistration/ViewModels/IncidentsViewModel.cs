using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class IncidentsViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;
        private readonly ILocationService _locationService;
        private readonly IResponseTeamService _teamService;

        public ObservableCollection<Incident> Incidents { get; } = new();

        public Action<Incident>? OnShowDetailsRequested;
        public Action? OnAddRequested;
        public Action<Incident>? OnUpdateRequested;

        [ObservableProperty]
        private Incident? selectedIncident;

        public IncidentsViewModel(
            IIncidentService incidentService,
            ILocationService locationService,
            IResponseTeamService teamService)
        {
            _incidentService = incidentService;
            _locationService = locationService;
            _teamService = teamService;
        }

        [RelayCommand]
        public void LoadData()
        {
            Incidents.Clear();
            var data = _incidentService.GetAll();
            foreach (var incident in data)
            {
                Incidents.Add(incident);
            }
        }

        [RelayCommand]
        private void Details()
        {
            if (SelectedIncident != null)
            {
                OnShowDetailsRequested?.Invoke(SelectedIncident);
            }
            else
            {
                MessageBox.Show("Выберите инцидент из списка для просмотра деталей.");
            }
        }

        [RelayCommand]
        private void Add()
        {
            OnAddRequested?.Invoke();
        }

        [RelayCommand]
        private void Refresh() => LoadData();

        [RelayCommand]
        private void Delete()
        {
            if (SelectedIncident == null) return;
            MessageBox.Show("Функция удаления в разработке или требует прав админа.");
        }

        [RelayCommand]
        private void Update()
        {
            if (SelectedIncident != null)
            {
                OnUpdateRequested?.Invoke(SelectedIncident);
            }
            else
            {
                MessageBox.Show("Выберите инцидент для редактирования");
            }
        }
    }
}
