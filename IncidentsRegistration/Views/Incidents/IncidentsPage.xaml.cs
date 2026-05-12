using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace IncidentsRegistration.Views
{
    public partial class IncidentsPage : Page
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IncidentsViewModel _vm;

        public IncidentsPage(
            IncidentsViewModel vm,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _vm = vm;
            _serviceProvider = serviceProvider;

            DataContext = _vm;

            _vm.OnShowDetailsRequested = OpenDetails;
            _vm.OnAddRequested = OpenAdd;
            _vm.OnUpdateRequested = OpenUpdate;

            Loaded += (s, e) => _vm.LoadData();
        }

        private void OpenDetails(Models.Incident incident)
        {
            var vm =
                _serviceProvider.GetRequiredService<IncidentDetailsViewModel>();

            vm.Initialize(incident.IdIncident);

            var page =
                _serviceProvider.GetRequiredService<IncidentDetailsPage>();

            page.DataContext = vm;

            NavigationService.Navigate(page);
        }

        private void OpenAdd()
        {
            var vm =
                _serviceProvider.GetRequiredService<AddIncidentViewModel>();

            var page =
                _serviceProvider.GetRequiredService<AddIncidentPage>();

            page.DataContext = vm;

            NavigationService.Navigate(page);
        }

        private void OpenUpdate(Models.Incident incident)
        {
            var vm =
                _serviceProvider.GetRequiredService<AddIncidentViewModel>();

            vm.Initialize(incident);

            var page =
                _serviceProvider.GetRequiredService<AddIncidentPage>();

            page.DataContext = vm;

            NavigationService.Navigate(page);
        }

        private void SortByTime_Click(object sender, RoutedEventArgs e)
            => ApplySort("RegistrationTime", ListSortDirection.Ascending);

        private void SortByType_Click(object sender, RoutedEventArgs e)
            => ApplySort("TypeOfIncident", ListSortDirection.Ascending);

        private void SortByTeam_Click(object sender, RoutedEventArgs e)
            => ApplySort("IdResponseTeamNavigation.NameTeam", ListSortDirection.Ascending);

        private void ApplySort(string propertyName, ListSortDirection direction)
        {
            var view = CollectionViewSource.GetDefaultView(_vm.Incidents);

            if (view == null)
                return;

            view.SortDescriptions.Clear();

            view.SortDescriptions.Add(
                new SortDescription(propertyName, direction));
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(_vm.Incidents);

            if (view == null)
                return;

            string searchText = SearchBox.Text.ToLower().Trim();

            view.Filter = obj =>
            {
                if (obj is not Models.Incident incident)
                    return false;

                if (string.IsNullOrWhiteSpace(searchText))
                    return true;

                return
                    (incident.TypeOfIncident?
                        .ToLower()
                        .Contains(searchText) ?? false)

                    ||

                    (incident.IdResponseTeamNavigation?
                        .NameTeam?
                        .ToLower()
                        .Contains(searchText) ?? false);
            };
        }
    }
}