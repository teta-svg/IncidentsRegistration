using IncidentsRegistration.Data;
using IncidentsRegistration.Services;
using IncidentsRegistration.ViewModels;
using System.Windows;
using System.Windows.Navigation;

namespace IncidentsRegistration.Views
{
    public partial class MainWindow : Window
    {
        private readonly IncidentsViewModel _incidentsVm;

        public MainWindow()
        {
            InitializeComponent();

            var context = new IncidentsDbContext();
            _incidentsVm = new IncidentsViewModel(
                new LocationService(context),
                new ResponseTeamService(context),
                new IncidentService(context));

            MainFrame.Navigate(new IncidentsPage(_incidentsVm));
        }

        private void NavigateIncidents_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new IncidentsPage(_incidentsVm));
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void NavigateTeams_Click(object sender, RoutedEventArgs e) { /* MainFrame.Navigate(new TeamsPage()); */ }
        private void NavigateLocations_Click(object sender, RoutedEventArgs e) { /* MainFrame.Navigate(new LocationsPage()); */ }
        private void NavigateReports_Click(object sender, RoutedEventArgs e) { /* MainFrame.Navigate(new ReportsPage()); */ }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack) MainFrame.GoBack();
        }
    }
}
