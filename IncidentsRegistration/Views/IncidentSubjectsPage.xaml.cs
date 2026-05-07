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
                var detailsVm = new SubjectDetailsViewModel(incident);
                NavigationService.Navigate(new SubjectDetailsPage(detailsVm));
            };


            this.Loaded += (s, e) => vm.LoadData();
        }

    }
}
