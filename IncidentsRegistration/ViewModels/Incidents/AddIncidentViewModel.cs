using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;

namespace IncidentsRegistration.ViewModels
{
    public partial class AddIncidentViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;
        private readonly ILocationService _locationService;
        private readonly IResponseTeamService _teamService;
        private readonly IContentNavigationService _nav;

        [ObservableProperty]
        private Incident _newIncident;

        [ObservableProperty]
        private Location _newLocation;

        [ObservableProperty]
        private DateTime _displayDate = DateTime.Today;

        [ObservableProperty]
        private DateTime _displayTime = DateTime.Now;

        [ObservableProperty]
        private string _pageTitle = "РЕГИСТРАЦИЯ ИНЦИДЕНТА";

        [ObservableProperty]
        private string? _errorMessage;

        public ObservableCollection<ResponseTeam> FreeTeams { get; } = new();

        [ObservableProperty]
        private ResponseTeam? _selectedTeam;

        private bool _isEditMode;

        public AddIncidentViewModel(
            IIncidentService incidentService,
            ILocationService locationService,
            IResponseTeamService teamService,
            IContentNavigationService nav)
        {
            _incidentService = incidentService;
            _locationService = locationService;
            _teamService = teamService;
            _nav = nav;

            _newIncident = new Incident();
            _newLocation = new Location();

            LoadTeams();
        }

        public void Initialize(Incident existingIncident)
        {
            _isEditMode = true;
            PageTitle = "РЕДАКТИРОВАНИЕ ИНЦИДЕНТА";
            NewIncident = existingIncident;
            NewLocation = existingIncident.IncidentLocations.FirstOrDefault()?.IdLocationNavigation ?? new Location();

            DisplayDate = NewIncident.RegistrationDate.ToDateTime(TimeOnly.MinValue);
            DisplayTime = DateTime.Today.Add(NewIncident.RegistrationTime.ToTimeSpan());

            LoadTeams();
        }

        private void LoadTeams()
        {
            FreeTeams.Clear();

            var teams = _isEditMode
                ? _teamService.GetAllTeams()
                : _teamService.GetFreeTeams();

            foreach (var team in teams)
            {
                FreeTeams.Add(team);
            }

            SelectedTeam = FreeTeams.FirstOrDefault(t => t.IdResponseTeam == NewIncident.IdResponseTeam);
        }

        [RelayCommand]
        private void Save()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(NewIncident.TypeOfIncident))
            {
                ErrorMessage = "Поле 'Тип инцидента' не может быть пустым";
                return;
            }

            if (string.IsNullOrWhiteSpace(NewLocation.Region) ||
                string.IsNullOrWhiteSpace(NewLocation.Settlement) ||
                string.IsNullOrWhiteSpace(NewLocation.Street))
            {
                ErrorMessage = "Заполните адрес";
                return;
            }

            if (DisplayDate > DateTime.Today)
            {
                ErrorMessage = "Дата регистрации не может быть в будущем";
                return;
            }

            if (SelectedTeam == null)
            {
                ErrorMessage = "Необходимо назначить группу реагирования";
                return;
            }

            try
            {
                NewIncident.RegistrationDate = DateOnly.FromDateTime(DisplayDate);
                NewIncident.RegistrationTime = TimeOnly.FromDateTime(DisplayTime);

                NewIncident.IdResponseTeam = SelectedTeam?.IdResponseTeam;
                NewIncident.IdResponseTeamNavigation = null;

                if (_isEditMode)
                {
                    _incidentService.UpdateIncident(NewIncident, NewLocation);
                    ErrorMessage = "Данные успешно обновлены";
                }
                else
                {
                    var actualLocation =
                        _locationService.GetOrCreateLocation(NewLocation);

                    _incidentService.CreateIncident(
                        NewIncident,
                        actualLocation.IdLocation);

                    ErrorMessage = "Инцидент успешно зарегистрирован";
                }

                _nav.GoBack();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerException?.Message ?? ex.Message;
            }
        }

        [RelayCommand]
        private void Back()
        {
            _nav.GoBack();
        }
    }
}