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
                string currentRole = _vm.Role?.ToLower().Trim();

                if (currentRole == "руководитель группы")
                {
                    OpenActiveIncidents();
                }
                else
                {
                    OpenAllIncidents();
                }
            };
        }

        private T GetService<T>() where T : class
            => ((App)Application.Current).Services.GetService<T>();

        private void OpenActiveIncidents()
        {
            var incidentService = GetService<IIncidentService>();
            var activeVm = new ActiveIncidentsViewModel(incidentService, _vm.CurrentUser);
            MainFrame.Navigate(new ActiveIncidentsPage(activeVm));
        }

        private void OpenAllIncidents()
        {
            var incidentsVm = GetService<IncidentsViewModel>();
            if (incidentsVm != null)
                MainFrame.Navigate(new IncidentsPage(incidentsVm));
        }

        private void Incidents_Click(object sender, RoutedEventArgs e) => OpenAllIncidents();

        private void ActiveIncidents_Click(object sender, RoutedEventArgs e) => OpenActiveIncidents();

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
