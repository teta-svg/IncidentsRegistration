using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace IncidentsRegistration.ViewModels
{
    public partial class IncidentsViewModel : ObservableObject
    {
        private readonly ILocationService _locationService;
        private readonly IResponseTeamService _responseTeamService;
        private readonly IIncidentService _incidentService;

        public ObservableCollection<Incident> Incidents { get; } = new();
        public ObservableCollection<Location> Locations { get; } = new();
        public ObservableCollection<ResponseTeam> FreeResponseTeams { get; } = new();


        [ObservableProperty]
        private Incident selectedIncident;

        [ObservableProperty]
        private Location selectedLocation;

        [ObservableProperty]
        private ResponseTeam selectedResponseTeam;

        public IncidentsViewModel(
            ILocationService locationService,
            IResponseTeamService responseTeamService,
            IIncidentService incidentService)
        {
            _locationService = locationService;
            _responseTeamService = responseTeamService;
            _incidentService = incidentService;
        }

        [RelayCommand]
        private void LoadData()
        {
            Incidents.Clear();

            foreach (var incident in _incidentService.GetAll())
                Incidents.Add(incident);

            FreeResponseTeams.Clear();

            foreach (var team in _responseTeamService.GetFreeTeams())
                FreeResponseTeams.Add(team);

        }

        [RelayCommand]
        private void AssignTeam()
        {
            if (SelectedIncident == null || SelectedResponseTeam == null)
                return;

            _responseTeamService.AssignTeam(
                SelectedIncident.IdIncident, 
                SelectedResponseTeam.IdResponseTeam);

            SelectedResponseTeam = null;
            SelectedIncident = null;

            LoadData();

            CollectionViewSource.GetDefaultView(Incidents).Refresh();
        }

        [RelayCommand]
        private void AddLocation()
        {
            if (SelectedIncident == null || SelectedLocation == null)
                return;
            _locationService.AttachToIncident(
                SelectedIncident.IdIncident, 
                SelectedLocation.IdLocation);

            LoadData();
        }
    }
}
