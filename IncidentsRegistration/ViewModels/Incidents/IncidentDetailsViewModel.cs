using CommunityToolkit.Mvvm.ComponentModel;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class IncidentDetailsViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;

        [ObservableProperty]
        private Incident? _selectedIncident;

        public IncidentDetailsViewModel(IIncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        public void Initialize(int incidentId)
        {
            SelectedIncident = _incidentService.GetFullIncidentDetails(incidentId);


            //уведомления об обновлении
            OnPropertyChanged(nameof(SelectedIncident));
            OnPropertyChanged(nameof(IncidentTitle));
            OnPropertyChanged(nameof(DirectorFullName));
        }


        public string IncidentTitle 
            => $"Инцидент №{SelectedIncident?.IdIncident}";

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
            SelectedIncident?.Decision?.CriminalCase != null ? 
            Visibility.Visible : Visibility.Collapsed;//если есть уголовное дело

        public Visibility TransferVisibility =>
            SelectedIncident?.Decision?.TerritorialTransfer != null ? 
            Visibility.Visible : Visibility.Collapsed;//если есть территориальная передача
    }
}
