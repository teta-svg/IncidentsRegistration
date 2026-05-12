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
        private Incident _currentIncident;

        [ObservableProperty]
        private ObservableCollection<SubjectRole> _uniqueParticipants;

        public Action<int, Subject>? OnUpdateParticipantRequested;

        public SubjectDetailsViewModel(Incident incident, ISubjectService subjectService)
        {
            _currentIncident = incident;
            _subjectService = subjectService;
            LoadParticipants();
        }
        public void LoadParticipants()
        {
            var allRoles = _subjectService.GetParticipantsByIncident(CurrentIncident.IdIncident);

            var latestStats = allRoles
                .GroupBy(r => r.IdSubject)
                .Select(group => group
                    .OrderByDescending(r => r.DateOfInvolvement)
                    .ThenByDescending(r => r.IdSubjectRole)
                    .First())
                .ToList();

            UniqueParticipants = new ObservableCollection<SubjectRole>(latestStats);
        }



        [RelayCommand]
        private void RemoveParticipant(SubjectRole role)
        {
            if (role == null) return;

            var result = MessageBox.Show($"Удалить участие {role.IdSubjectNavigation.LastName}?", "Удаление", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _subjectService.RemoveRolesForPersonInIncident(role.IdSubjectRole);
                    LoadParticipants();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
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