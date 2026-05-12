using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class MainAppPage : Page
    {
        private readonly MainAppViewModel _vm;
        private readonly IServiceProvider _serviceProvider;

        public MainAppPage(MainAppViewModel vm, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _vm = vm;
            _serviceProvider = serviceProvider;
            DataContext = _vm;

            Loaded += (s, e) =>
            {
                if (_vm.Role == "руководитель группы")
                    NavigateToActiveIncidents();
                else
                    NavigateToIncidents();
            };
        }


        private void NavigateToIncidents()
        {
            var page = _serviceProvider.GetRequiredService<IncidentsPage>();
            MainFrame.Navigate(page);
        }

        private void NavigateToActiveIncidents()
        {
            var page = _serviceProvider.GetRequiredService<ActiveIncidentsPage>();

            page.Initialize(_vm.CurrentUser);

            MainFrame.Navigate(page);
        }

        private void Incidents_Click(object sender, RoutedEventArgs e) => NavigateToIncidents();

        private void ActiveIncidents_Click(object sender, RoutedEventArgs e) => NavigateToActiveIncidents();

        private void Subjects_Click(object sender, RoutedEventArgs e)
        {
            var page = _serviceProvider.GetRequiredService<IncidentSubjectsPage>();
            page.Initialize(_vm.CurrentUser);
            MainFrame.Navigate(page);
        }


        private void Teams_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(_serviceProvider.GetRequiredService<ResponseTeamsPage>());

        private void Users_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(_serviceProvider.GetRequiredService<UsersPage>());

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Выход",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var loginPage = _serviceProvider.GetRequiredService<LoginPage>();
                NavigationService.Navigate(loginPage);
            }
        }
    }
}