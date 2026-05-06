using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Windows;
using System.Windows.Navigation;

namespace IncidentsRegistration.ViewModels
{
    public partial class IncidentDetailsViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;

        [ObservableProperty]
        private Incident? _selectedIncident;

        public IncidentDetailsViewModel(IIncidentService incidentService, int incidentId)
        {
            _incidentService = incidentService;
            SelectedIncident = _incidentService.GetFullIncidentDetails(incidentId);
        }

        public string IncidentTitle => $"Инцидент №{SelectedIncident?.IdIncident}";

        public string DirectorFullName
        {
            get
            {
                var team = SelectedIncident?.IdResponseTeamNavigation;

                if (team == null) return "не назначен";

                string full = $"{team.DirectorLastName?.Trim()} {team.DirectorFirstName?.Trim()} {team.DirectorPatronymic?.Trim()}".Trim();

                return string.IsNullOrWhiteSpace(full) ? "не указано" : full;
            }
        }


        public Visibility CaseVisibility =>
            SelectedIncident?.Decision?.CriminalCase != null ? Visibility.Visible : Visibility.Collapsed;

        public Visibility TransferVisibility =>
            SelectedIncident?.Decision?.TerritorialTransfer != null ? Visibility.Visible : Visibility.Collapsed;

        [RelayCommand]
        private void GoBack(NavigationService nav)
        {
            if (nav != null && nav.CanGoBack) nav.GoBack();
        }
    }
}
