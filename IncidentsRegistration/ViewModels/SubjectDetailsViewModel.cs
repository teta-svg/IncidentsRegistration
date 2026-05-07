using CommunityToolkit.Mvvm.ComponentModel;
using IncidentsRegistration.Models;

namespace IncidentsRegistration.ViewModels
{
    public partial class SubjectDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private Incident _currentIncident;

        public SubjectDetailsViewModel(Incident incident)
        {
            CurrentIncident = incident;
        }
    }

}
