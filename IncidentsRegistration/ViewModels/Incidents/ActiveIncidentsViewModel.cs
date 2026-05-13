using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using IncidentsRegistration.Views;
using System.Collections.ObjectModel;

namespace IncidentsRegistration.ViewModels
{
    public partial class ActiveIncidentsViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;
        private readonly IContentNavigationService _nav;

        public ObservableCollection<Incident> ActiveIncidents { get; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MakeDecisionCommand))]
        private Incident? selectedIncident;

        [ObservableProperty]
        private SystemUser? _currentUser;

        [ObservableProperty]
        private string? _errorMessage;

        public ActiveIncidentsViewModel(IIncidentService incidentService, IContentNavigationService nav)
        {
            _incidentService = incidentService;
            _nav = nav;
        }

        public void Initialize(SystemUser user)
        {
            CurrentUser = user;
            LoadData();
        }

        [RelayCommand]
        public void LoadData()
        {
            SelectedIncident = null;
            ErrorMessage = string.Empty;

            try
            {
                ActiveIncidents.Clear();

                if (CurrentUser == null) return;

                var teamId = CurrentUser.SystemUserResponseTeams.FirstOrDefault()?.IdResponseTeam;

                if (teamId.HasValue)
                {
                    var data = _incidentService.GetActiveIncidentsByTeam(teamId.Value);
                    foreach (var item in data)
                        ActiveIncidents.Add(item);

                    if (ActiveIncidents.Count == 0)
                        ErrorMessage = "Активных инцидентов для вашей команды нет.";
                }
                else
                {
                    ErrorMessage = "Вы не привязаны ни к одной группе реагирования.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка при загрузке данных: " + ex.Message;
            }
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

            _nav.Navigate<AddDecisionPage>(SelectedIncident.IdIncident);
        }

        private bool HasSelection() => SelectedIncident != null;
    }
}