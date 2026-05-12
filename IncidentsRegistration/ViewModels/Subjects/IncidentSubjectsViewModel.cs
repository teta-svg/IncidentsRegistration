using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;

namespace IncidentsRegistration.ViewModels
{
    public partial class IncidentSubjectsViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;
        private readonly ISubjectService _subjectService;

        [ObservableProperty]
        private SystemUser? _currentUser;

        public ObservableCollection<Incident> Incidents { get; } = new();
        public ObservableCollection<SubjectRole> Participants { get; } = new();

        [ObservableProperty]
        private Incident? _selectedIncident;

        [ObservableProperty]
        private string? _errorMessage;

        public Action<int>? OnAddParticipantRequested;
        public Action<Incident>? OnShowDetailsRequested;

        public IncidentSubjectsViewModel(
            IIncidentService service,
            ISubjectService subjectService)
        {
            _incidentService = service;
            _subjectService = subjectService;
        }

        public void Initialize(SystemUser user)
        {
            CurrentUser = user;
            LoadData();
        }

        [RelayCommand]
        public void LoadData()
        {
            ErrorMessage = string.Empty;

            try
            {
                Incidents.Clear();
                Participants.Clear();

                if (CurrentUser == null)
                    return;

                var teamLink = CurrentUser.SystemUserResponseTeams.FirstOrDefault();

                if (teamLink == null)
                {
                    ErrorMessage = "Вы не привязаны к группе реагирования.";
                    return;
                }

                var data = _incidentService.GetAll()
                    .Where(i => i.IdResponseTeam == teamLink.IdResponseTeam)
                    .ToList();

                foreach (var incident in data)
                {
                    Incidents.Add(incident);
                }

                if (Incidents.Count == 0)
                {
                    ErrorMessage = "Инцидентов вашей группы не найдено.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка загрузки: "
                    + (ex.InnerException?.Message ?? ex.Message);
            }
        }

        partial void OnSelectedIncidentChanged(Incident? value)
        {
            Participants.Clear();
            ErrorMessage = string.Empty;

            if (value == null)
                return;

            try
            {
                var allRoles = _subjectService
                    .GetParticipantsByIncident(value.IdIncident);

                var latestParticipants = allRoles
                    .GroupBy(r => r.IdSubject)
                    .Select(g => g
                        .OrderByDescending(r => r.DateOfInvolvement)
                        .ThenByDescending(r => r.IdSubjectRole)
                        .First())
                    .ToList();

                foreach (var participant in latestParticipants)
                {
                    Participants.Add(participant);
                }

                if (!Participants.Any())
                {
                    ErrorMessage = "У данного инцидента пока нет участников.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка загрузки участников: "
                    + (ex.InnerException?.Message ?? ex.Message);
            }
        }

        [RelayCommand]
        private void ShowSubjectDetails()
        {
            ErrorMessage = string.Empty;
            if (SelectedIncident == null)
            {
                ErrorMessage = "Выберите инцидент в списке!";
                return;
            }
            OnShowDetailsRequested?.Invoke(SelectedIncident);
        }

        [RelayCommand]
        private void AddParticipant()
        {
            ErrorMessage = string.Empty;
            if (SelectedIncident == null)
            {
                ErrorMessage = "Сначала выберите инцидент, к которому добавить участника";
                return;
            }
            OnAddParticipantRequested?.Invoke(SelectedIncident.IdIncident);
        }
    }
}