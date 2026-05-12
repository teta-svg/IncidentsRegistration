using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class SubjectDetailsViewModel : ObservableObject
    {
        private readonly ISubjectService _subjectService;

        [ObservableProperty]
        private Incident _currentIncident = new();

        [ObservableProperty]
        private string? _errorMessage;

        public ObservableCollection<SubjectRole> UniqueParticipants { get; } = new();

        public Action<int, Subject>? OnUpdateParticipantRequested;

        public SubjectDetailsViewModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public void Initialize(Incident incident)
        {
            CurrentIncident = incident;
            ErrorMessage = string.Empty;
            LoadParticipants();
        }

        public void LoadParticipants()
        {
            try
            {
                UniqueParticipants.Clear();
                var allRoles = _subjectService.GetParticipantsByIncident(CurrentIncident.IdIncident);

                var latestStats = allRoles
                    .GroupBy(r => r.IdSubject)
                    .Select(group => group
                        .OrderByDescending(r => r.DateOfInvolvement)
                        .ThenByDescending(r => r.IdSubjectRole)
                        .First())
                    .ToList();

                foreach (var participant in latestStats)
                {
                    UniqueParticipants.Add(participant);
                }

                if (UniqueParticipants.Count == 0)
                    ErrorMessage = "У данного инцидента нет зарегистрированных участников.";
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка загрузки участников: " + (ex.InnerException?.Message ?? ex.Message);
            }
        }

        [RelayCommand]
        private void RemoveParticipant(SubjectRole role)
        {
            if (role?.IdSubjectNavigation == null) return;
            ErrorMessage = string.Empty;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить участие {role.IdSubjectNavigation.LastName} из данного инцидента?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _subjectService.RemoveRolesForPersonInIncident(role.IdSubjectRole);
                    LoadParticipants();
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Ошибка при удалении: " + (ex.InnerException?.Message ?? ex.Message);
                }
            }
        }

        [RelayCommand]
        private void UpdateParticipant(SubjectRole role)
        {
            if (role?.IdSubjectNavigation == null) return;
            OnUpdateParticipantRequested?.Invoke(CurrentIncident.IdIncident, role.IdSubjectNavigation);
        }
    }
}