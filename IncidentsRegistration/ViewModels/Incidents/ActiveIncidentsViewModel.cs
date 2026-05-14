using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using IncidentsRegistration.Services;
using IncidentsRegistration.Views;
using System.Collections.ObjectModel;

namespace IncidentsRegistration.ViewModels
{
    public partial class ActiveIncidentsViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;
        private readonly IContentNavigationService _nav;
        private readonly IUserService _userService;

        public ObservableCollection<Incident> ActiveIncidents { get; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MakeDecisionCommand))]
        private Incident? selectedIncident;

        [ObservableProperty]
        private SystemUser? _currentUser;

        [ObservableProperty]
        private string? _errorMessage;

        public ActiveIncidentsViewModel(
            IIncidentService incidentService,
            IContentNavigationService nav,
            IUserService userService)
        {
            _incidentService = incidentService;
            _nav = nav;
            _userService = userService;
        }

        public void Initialize()
        {
            CurrentUser = _userService.CurrentUser;
            LoadData();
        }

        [RelayCommand]
        public void LoadData()
        {
            ErrorMessage = string.Empty;
            ActiveIncidents.Clear();

            var user = _userService.CurrentUser;
            if (user == null) return;

            var teamId = user.SystemUserResponseTeams.FirstOrDefault()?.IdResponseTeam;

            if (!teamId.HasValue)
            {
                ErrorMessage = "Вы не привязаны ни к одной группе реагирования.";
                return;
            }

            var data = _incidentService.GetActiveIncidentsByTeam(teamId.Value);

            foreach (var item in data)
                ActiveIncidents.Add(item);

            if (ActiveIncidents.Count == 0)
                ErrorMessage = "Активных инцидентов для вашей команды нет.";
        }

        [RelayCommand(CanExecute = nameof(HasSelection))]
        private void MakeDecision()
        {
            ErrorMessage = string.Empty;

            if (SelectedIncident == null)
            {
                ErrorMessage = "Выберите инцидент из списка.";
                return;
            }

            _nav.Navigate<AddDecisionPage>(new DecisionInitDTO
            {
                IncidentId = SelectedIncident.IdIncident,
                User = _userService.CurrentUser
            });
        }

        private bool HasSelection() => SelectedIncident != null;
    }
}