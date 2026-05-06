using IncidentsRegistration.Interfaces;
using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class MainAppPage : Page
    {
        private readonly MainAppViewModel _vm;
        public MainAppPage(MainAppViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;

            Loaded += (s, e) =>
            {
                if (_vm.Role == "Team")
                {
                    OpenActiveIncidents();
                }
                else
                {
                    OpenAllIncidents();
                }
            };
        }

        private void ActiveIncidents_Click(object sender, RoutedEventArgs e) => OpenActiveIncidents();

        private void OpenActiveIncidents()
        {
            var services = ((App)Application.Current).Services;
            var incidentService = services.GetService<IIncidentService>();

            var activeVm = new ActiveIncidentsViewModel(incidentService, _vm.CurrentUser);
            MainFrame.Navigate(new ActiveIncidentsPage(activeVm));
        }

        private void OpenAllIncidents()
        {
            var services = ((App)Application.Current).Services;
            var incidentsVm = services.GetService<IncidentsViewModel>();
            if (incidentsVm != null)
                MainFrame.Navigate(new IncidentsPage(incidentsVm));
        }


        private void Incidents_Click(object sender, RoutedEventArgs e)
        {
            var services = ((App)Application.Current).Services;

            var incidentsVm = services.GetService<IncidentsViewModel>();

            if (incidentsVm != null)
            {
                MainFrame.Navigate(new IncidentsPage(incidentsVm));
            }
            else
            {
                MessageBox.Show("Ошибка");
            }
        }
        private void Subjects_Click(object sender, RoutedEventArgs e)
        {
            // MainFrame.Navigate(new SubjectsPage());
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            // MainFrame.Navigate(new ReportsPage());
        }

        private void Teams_Click(object sender, RoutedEventArgs e)
        {
            // MainFrame.Navigate(new TeamsManagementPage());
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            // MainFrame.Navigate(new UsersManagementPage());
        }
    }
}
