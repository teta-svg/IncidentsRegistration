using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;
using System.Windows;

public partial class AddIncidentViewModel : ObservableObject
{
    private readonly IIncidentService _incidentService;
    private readonly ILocationService _locationService;
    private readonly IResponseTeamService _teamService;

    [ObservableProperty] private Incident _newIncident;
    [ObservableProperty] private Location _newLocation;
    [ObservableProperty] private DateTime _displayDate = DateTime.Today;
    [ObservableProperty] private DateTime _displayTime = DateTime.Now;
    [ObservableProperty] private string _pageTitle = "РЕГИСТРАЦИЯ ИНЦИДЕНТА";

    public ObservableCollection<ResponseTeam> FreeTeams { get; } = new();
    [ObservableProperty] private ResponseTeam? _selectedTeam;

    private bool _isEditMode;

    public AddIncidentViewModel(
            IIncidentService incidentService,
            ILocationService locationService,
            IResponseTeamService teamService)
    {
        _incidentService = incidentService;
        _locationService = locationService;
        _teamService = teamService;

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
        try
        {
            NewIncident.RegistrationDate = DateOnly.FromDateTime(DisplayDate);
            NewIncident.RegistrationTime = TimeOnly.FromDateTime(DisplayTime);
            NewIncident.IdResponseTeam = SelectedTeam?.IdResponseTeam;

            if (_isEditMode)
            {
                _incidentService.UpdateIncident(NewIncident, NewLocation);
                MessageBox.Show("Данные успешно обновлены");
            }
            else
            {
                var actualLocation = _locationService.GetOrCreateLocation(NewLocation);
                _incidentService.CreateIncident(NewIncident, actualLocation.IdLocation);
                MessageBox.Show("Инцидент успешно зарегистрирован");
            }
        }
        catch (Exception ex)
        {
            var fullMessage = ex.InnerException?.Message ?? ex.Message;
            MessageBox.Show($"Ошибка: {fullMessage}");
        }
    }
}
