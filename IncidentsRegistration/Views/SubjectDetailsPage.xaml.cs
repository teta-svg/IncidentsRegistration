using IncidentsRegistration.Interfaces;
using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace IncidentsRegistration.Views
{
    public partial class SubjectDetailsPage : Page
    {
        public SubjectDetailsPage(SubjectDetailsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            var services = ((App)Application.Current).Services;
            vm.OnUpdateParticipantRequested = (incidentId, subject) =>
            {
                var subjectService = services.GetRequiredService<ISubjectService>();
                var editVm = new AddSubjectViewModel(subjectService, incidentId, subject);

                var nav = this.NavigationService;

                editVm.OnSuccess = () =>
                {
                    if (nav != null && nav.CanGoBack) nav.GoBack();
                    vm.LoadParticipants();
                };


                NavigationService.Navigate(new AddSubjectPage(editVm));
            };

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
