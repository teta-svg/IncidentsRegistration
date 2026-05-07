using IncidentsRegistration.Interfaces;
using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class IncidentsPage : Page
    {
        public IncidentsPage(IncidentsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            var services = ((App)Application.Current).Services;

            vm.OnShowDetailsRequested = (incident) => {
                var incidentService = services.GetRequiredService<IIncidentService>();
                var detailsVm = new IncidentDetailsViewModel(incidentService, incident.IdIncident);
                NavigationService.Navigate(new IncidentDetailsPage(detailsVm));
            };

            vm.OnAddRequested = () => {
                var addPage = services.GetRequiredService<AddIncidentPage>();
                NavigationService.Navigate(addPage);
            };

            vm.OnUpdateRequested = (incident) =>
            {
                var editVm = services.GetRequiredService<AddIncidentViewModel>();
                editVm.Initialize(incident);

                NavigationService.Navigate(new AddIncidentPage(editVm));
            };

            this.Loaded += (s, e) => vm.LoadData();
        }

    }
}
