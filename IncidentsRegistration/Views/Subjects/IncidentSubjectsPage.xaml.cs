using IncidentsRegistration.Models;
using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class IncidentSubjectsPage : Page
    {
        private readonly IServiceProvider _serviceProvider;

        public IncidentSubjectsPage(
            IncidentSubjectsViewModel vm,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            DataContext = vm;
            _serviceProvider = serviceProvider;

            vm.OnAddParticipantRequested = (incidentId) =>
            {
                var addVm = _serviceProvider.GetRequiredService<AddSubjectViewModel>();

                addVm.Initialize(incidentId);

                addVm.OnSuccess = () =>
                {
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();

                    vm.LoadData();
                };

                var addPage = _serviceProvider.GetRequiredService<AddSubjectPage>();

                addPage.DataContext = addVm;

                NavigationService.Navigate(addPage);
            };

            vm.OnShowDetailsRequested = (incident) =>
            {
                var detailsVm = _serviceProvider.GetRequiredService<SubjectDetailsViewModel>();

                detailsVm.Initialize(incident);

                var detailsPage = _serviceProvider.GetRequiredService<SubjectDetailsPage>();

                detailsPage.DataContext = detailsVm;

                NavigationService.Navigate(detailsPage);
            };
        }

        public void Initialize(SystemUser user)
        {
            ((IncidentSubjectsViewModel)DataContext).Initialize(user);
        }
    }
}
