using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class IncidentSubjectsViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;
        private readonly SystemUser _currentUser;

        [ObservableProperty] private ObservableCollection<Incident> _incidents;
        [ObservableProperty] private Incident? _selectedIncident;

        public Action<int>? OnAddParticipantRequested;

        public Action<Incident>? OnShowDetailsRequested;

        public IncidentSubjectsViewModel(IIncidentService service, SystemUser user)
        {
            _incidentService = service;
            _currentUser = user;
            LoadData();
        }
        public void LoadData()
        {
            var teamLink = _currentUser.SystemUserResponseTeams.FirstOrDefault();
            if (teamLink != null)
            {
                var data = _incidentService.GetAll()
                    .Where(i => i.IdResponseTeam == teamLink.IdResponseTeam)
                    .ToList();

                Incidents = new ObservableCollection<Incident>(data);
            }
        }


        [RelayCommand]
        private void ShowSubjectDetails()
        {
            if (SelectedIncident == null)
            {
                MessageBox.Show("Выберите инцидент!");
                return;
            }
            OnShowDetailsRequested?.Invoke(SelectedIncident);
        }


        [RelayCommand]
        private void AddParticipant()
        {
            if (SelectedIncident == null) return;
            OnAddParticipantRequested?.Invoke(SelectedIncident.IdIncident);
        }
    }

}
