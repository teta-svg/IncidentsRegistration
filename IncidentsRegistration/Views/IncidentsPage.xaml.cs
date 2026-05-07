using IncidentsRegistration.Interfaces;
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
        public IncidentsPage(IncidentsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            var services = ((App)Application.Current).Services;

            vm.OnShowDetailsRequested = (incident) => {
                var incidentService = services.GetRequiredService<IIncidentService>();
                var detailsVm = new IncidentDetailsViewModel(incidentService, incident.IdIncident);
                NavigationService.Navigate(new IncidentDetailsPage(detailsVm));
            };

            vm.OnAddRequested = () => {
                var addPage = services.GetRequiredService<AddIncidentPage>();
                NavigationService.Navigate(addPage);
            };

            vm.OnUpdateRequested = (incident) =>
            {
                var editVm = services.GetRequiredService<AddIncidentViewModel>();
                editVm.Initialize(incident);

                NavigationService.Navigate(new AddIncidentPage(editVm));
            };

            this.Loaded += (s, e) => vm.LoadData();
        }

        private void SortByTime_Click(object sender, RoutedEventArgs e)
        {
            ApplySort("RegistrationTime");
        }

        private void SortByType_Click(object sender, RoutedEventArgs e)
        {
            ApplySort("TypeOfIncident");
        }

        private void SortByTeam_Click(object sender, RoutedEventArgs e)
        {
            ApplySort("IdResponseTeamNavigation.NameTeam");
        }

        private void ApplySort(string propertyName)
        {
            var view = CollectionViewSource.GetDefaultView(this.DataContext.GetType().GetProperty("Incidents").GetValue(this.DataContext));

            if (view != null)
            {
                view.SortDescriptions.Clear();
                ListSortDirection direction = (propertyName == "RegistrationTime") ? ListSortDirection.Descending : ListSortDirection.Ascending;
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
            }
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = DataContext as IncidentsViewModel;
            if (vm == null || vm.Incidents == null) return;
            var view = CollectionViewSource.GetDefaultView(vm.Incidents);

            if (view != null)
            {
                string searchText = SearchBox.Text.ToLower().Trim();

                view.Filter = (obj) =>
                {
                    var incident = obj as IncidentsRegistration.Models.Incident;

                    if (incident == null) return false;

                    if (string.IsNullOrWhiteSpace(searchText)) return true;

                    bool matchesType = incident.TypeOfIncident?.ToLower().Contains(searchText) ?? false;
                    bool matchesId = incident.IdIncident.ToString().Contains(searchText);
                    bool matchesTeam = incident.IdResponseTeamNavigation?.NameTeam?.ToLower().Contains(searchText) ?? false;

                    return matchesType || matchesId || matchesTeam;
                };
            }
        }


    }
}
