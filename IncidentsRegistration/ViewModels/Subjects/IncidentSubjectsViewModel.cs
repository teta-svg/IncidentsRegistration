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
        private readonly ISubjectService _subjectService;
        private readonly SystemUser _currentUser;

        [ObservableProperty] private ObservableCollection<Incident> _incidents;
        [ObservableProperty] private Incident? _selectedIncident;
        [ObservableProperty] private ObservableCollection<SubjectRole> _participants;

        public Action<int>? OnAddParticipantRequested;
        public Action<Incident>? OnShowDetailsRequested;
        public Action<int, Subject>? OnUpdateParticipantRequested;

        public IncidentSubjectsViewModel(IIncidentService service, ISubjectService subjectService, SystemUser user)
        {
            _incidentService = service;
            _subjectService = subjectService;
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

                foreach (var incident in data)
                {
                    if (incident.SubjectRoles != null)
                    {
                        incident.SubjectRoles = incident.SubjectRoles
                            .GroupBy(sr => sr.IdSubject)
                            .Select(g => g
                                .OrderByDescending(sr => sr.DateOfInvolvement)
                                .ThenByDescending(sr => sr.IdSubjectRole)
                                .First())
                            .ToList();
                    }
                }

                Incidents = new ObservableCollection<Incident>(data);
            }
        }


        partial void OnSelectedIncidentChanged(Incident? value)
        {
            if (value != null)
            {
                var allRoles = _subjectService.GetParticipantsByIncident(value.IdIncident);

                var latestParticipants = allRoles
                    .GroupBy(r => r.IdSubject)
                    .Select(g => g
                        .OrderByDescending(r => r.DateOfInvolvement)
                        .ThenByDescending(r => r.IdSubjectRole)
                        .First())
                    .ToList();

                Participants = new ObservableCollection<SubjectRole>(latestParticipants);
            }
            else
            {
                Participants = new ObservableCollection<SubjectRole>();
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