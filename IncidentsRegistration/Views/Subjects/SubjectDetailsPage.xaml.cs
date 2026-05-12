using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class SubjectDetailsPage : Page
    {
        private readonly IServiceProvider _serviceProvider;

        public SubjectDetailsPage(SubjectDetailsViewModel vm, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = vm;
            _serviceProvider = serviceProvider;

            vm.OnUpdateParticipantRequested = (incidentId, subject) =>
            {
                var editVm = _serviceProvider.GetRequiredService<AddSubjectViewModel>();
                editVm.Initialize(incidentId, subject);

                editVm.OnSuccess = () =>
                {
                    if (NavigationService.CanGoBack) 
                        NavigationService.GoBack();
                    vm.LoadParticipants();
                };

                var editPage = _serviceProvider.GetRequiredService<AddSubjectPage>();
                editPage.DataContext = editVm;

                NavigationService.Navigate(editPage);
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