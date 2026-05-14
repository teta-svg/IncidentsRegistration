using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using IncidentsRegistration.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class SubjectDetailsViewModel : ObservableObject
    {
        private readonly ISubjectService _subjectService;
        private readonly IContentNavigationService _nav;

        [ObservableProperty]
        private Incident _currentIncident;

        [ObservableProperty]
        private ObservableCollection<SubjectRole> _uniqueParticipants;

        public SubjectDetailsViewModel(
            ISubjectService subjectService,
            IContentNavigationService nav)
        {
            _subjectService = subjectService;
            _nav = nav;
        }

        public void Initialize(Incident incident)
        {
            CurrentIncident = incident;
            LoadParticipants();
        }

        public void LoadParticipants()
        {
            if (CurrentIncident == null) return;

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

            var name = role.IdSubjectNavigation?.LastName ?? "Неизвестно";

            var result = MessageBox.Show($"Удалить участие {name}?", "Удаление", MessageBoxButton.YesNo);
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
            if (role == null) return;

            _nav.Navigate<AddSubjectPage>(
                new SubjectEditPayloadDTO
                {
                    IncidentId = CurrentIncident.IdIncident,
                    Subject = role.IdSubjectNavigation,
                    CurrentRole = role
                });
        }


        [RelayCommand]
        private void Back()
        {
            _nav.GoBack();
        }
    }
}