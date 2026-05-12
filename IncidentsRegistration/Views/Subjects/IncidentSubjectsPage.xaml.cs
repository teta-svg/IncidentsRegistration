using IncidentsRegistration.Interfaces;
using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace IncidentsRegistration.Views
{
    public partial class IncidentSubjectsPage : Page
    {
        public IncidentSubjectsPage(IncidentSubjectsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            var services = ((App)Application.Current).Services;

            vm.OnAddParticipantRequested = (incidentId) =>
            {
                var subjectService = services.GetRequiredService<ISubjectService>();
                var addVm = new AddSubjectViewModel(subjectService, incidentId);

                var nav = NavigationService;

                addVm.OnSuccess = () => {
                    if (nav != null && nav.CanGoBack)
                    {
                        nav.GoBack();
                    }
                    vm.LoadData();
                };

                NavigationService.Navigate(new AddSubjectPage(addVm));
            };

            vm.OnShowDetailsRequested = (incident) =>
            {
                var subjectService = ((App)Application.Current).Services.GetRequiredService<ISubjectService>();
                var detailsVm = new SubjectDetailsViewModel(incident, subjectService);

                NavigationService.Navigate(new SubjectDetailsPage(detailsVm));
            };

            vm.OnUpdateParticipantRequested = (incidentId, subject) =>
            {
                var subjectService = services.GetRequiredService<ISubjectService>();
                var addVm = new AddSubjectViewModel(subjectService, incidentId, subject);

                addVm.OnSuccess = () => {
                    if (NavigationService.CanGoBack) NavigationService.GoBack();
                    vm.LoadData();
                };

                NavigationService.Navigate(new AddSubjectPage(addVm));
            };

            this.Loaded += (s, e) => vm.LoadData();
        }

    }
}