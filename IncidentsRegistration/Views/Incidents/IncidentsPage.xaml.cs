using IncidentsRegistration.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace IncidentsRegistration.Views
{
    public partial class IncidentsPage : Page
    {
        private readonly IncidentsViewModel _vm;

        private readonly AddIncidentPage _addPage;
        private readonly AddIncidentViewModel _addVm;
        private readonly IncidentDetailsViewModel _detailsVm;

        public IncidentsPage(
            IncidentsViewModel vm,
            AddIncidentPage addPage,
            AddIncidentViewModel addVm,
            IncidentDetailsViewModel detailsVm)
        {
            InitializeComponent();
            _vm = vm;
            _addPage = addPage;
            _addVm = addVm;
            _detailsVm = detailsVm;

            DataContext = _vm;

            _vm.OnShowDetailsRequested = incident
                => OpenDetails(incident);
            _vm.OnAddRequested = () 
                => NavigationService.Navigate(_addPage);
            _vm.OnUpdateRequested = incident 
                => OpenUpdate(incident);

            this.Loaded += (s, e) 
                => _vm.LoadData();
        }

        private void OpenDetails(Models.Incident incident)
        {
            _detailsVm.Initialize(incident.IdIncident);
            NavigationService.Navigate(new IncidentDetailsPage(_detailsVm));
        }

        private void OpenUpdate(Models.Incident incident)
        {
            _addVm.Initialize(incident);
            NavigationService.Navigate(new AddIncidentPage(_addVm));
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
            if (view == null) return;
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription(propertyName, direction));//правила сортировки
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(_vm.Incidents);
            if (view == null) return;

            string searchText = SearchBox.Text.ToLower().Trim();
            view.Filter = obj =>
            {
                if (obj is not Models.Incident incident) return false;
                if (string.IsNullOrWhiteSpace(searchText)) return true;

                return (incident.TypeOfIncident?.ToLower().Contains(searchText) ?? false) ||
                       (incident.IdResponseTeamNavigation?.NameTeam?.ToLower().Contains(searchText) ?? false);
            };
        }
    }
}