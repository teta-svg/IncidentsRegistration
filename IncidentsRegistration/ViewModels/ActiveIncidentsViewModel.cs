using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;

namespace IncidentsRegistration.ViewModels
{
    public partial class ActiveIncidentsViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;
        private readonly SystemUser _currentUser;

        public ObservableCollection<Incident> ActiveIncidents { get; } = new();

        public Action<int>? OnMakeDecisionRequested;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MakeDecisionCommand))]
        private Incident? selectedIncident;

        public ActiveIncidentsViewModel(IIncidentService incidentService, SystemUser currentUser)
        {
            _incidentService = incidentService;
            _currentUser = currentUser;
            LoadData();
        }

        [RelayCommand]
        public void LoadData()
        {
            ActiveIncidents.Clear();
            var teamId = _currentUser.SystemUserResponseTeams.FirstOrDefault()?.IdResponseTeam;

            if (teamId.HasValue)
            {
                var data = _incidentService.GetActiveIncidentsByTeam(teamId.Value);
                foreach (var item in data) ActiveIncidents.Add(item);
            }
        }

        [RelayCommand(CanExecute = nameof(HasSelection))]
        private void MakeDecision()
        {
            if (SelectedIncident != null)
            {
                OnMakeDecisionRequested?.Invoke(SelectedIncident.IdIncident);
            }
        }

        private bool HasSelection() => SelectedIncident != null;
    }
}
