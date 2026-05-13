using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using IncidentsRegistration.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class IncidentSubjectsViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;
        private readonly ISubjectService _subjectService;
        private readonly IUserService _userService;
        private readonly IContentNavigationService _nav;

        [ObservableProperty]
        private ObservableCollection<Incident> _incidents = new();

        [ObservableProperty]
        private Incident? _selectedIncident;

        [ObservableProperty]
        private ObservableCollection<SubjectRole> _participants = new();

        public IncidentSubjectsViewModel(
            IIncidentService service,
            ISubjectService subjectService,
            IUserService userService,
            IContentNavigationService nav)
        {
            _incidentService = service;
            _subjectService = subjectService;
            _userService = userService;
            _nav = nav;

            LoadData();
        }

        public void LoadData()
        {
            var user = _userService.CurrentUser;

            var teamLink = user?.SystemUserResponseTeams?.FirstOrDefault();

            if (teamLink == null)
            {
                Incidents.Clear();
                return;
            }

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

            _nav.Navigate<SubjectDetailsPage>(SelectedIncident);
        }

        [RelayCommand]
        private void AddParticipant()
        {
            if (SelectedIncident == null)
                return;

            _nav.Navigate<AddSubjectPage>(
                new SubjectEditPayloadDTO
                {
                    IncidentId = SelectedIncident.IdIncident,
                    Subject = null
                });
        }

        [RelayCommand]
        private void UpdateParticipant(Subject subject)
        {
            if (SelectedIncident == null)
                return;

            _nav.Navigate<AddSubjectPage>(
                new SubjectEditPayloadDTO
                {
                    IncidentId = SelectedIncident.IdIncident,
                    Subject = subject
                }
            );
        }
    }
}