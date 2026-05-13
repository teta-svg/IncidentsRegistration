using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using IncidentsRegistration.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace IncidentsRegistration.ViewModels
{
    public partial class IncidentsViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;
        private readonly IUserService _userService;
        private readonly IContentNavigationService _nav;
        private ICollectionView? _incidentsView;
        private ICollectionView View 
            => _incidentsView ??= CollectionViewSource.GetDefaultView(Incidents);

        partial void OnSearchTextChanged(string value)
            => View?.Refresh();

        public ObservableCollection<Incident> Incidents { get; } = new();

        [ObservableProperty]
        private Incident? selectedIncident;

        [ObservableProperty]
        private string role = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private string searchText = string.Empty;

        public IncidentsViewModel(
            IIncidentService incidentService,
            IUserService userService,
            IContentNavigationService nav)
        {
            _incidentService = incidentService;
            _userService = userService;
            _nav = nav;

            var currentUser = _userService.CurrentUser;

            if (currentUser != null)
            {
                Role = currentUser.UserRole?
                    .ToLower()
                    .Trim() ?? string.Empty;
            }
        }

        public bool CanEdit =>
                Role == "администратор" ||
                Role == "руководитель группы";

        public bool CanDelete =>
            Role == "администратор";

        [RelayCommand]
        public void LoadData()
        {
            ErrorMessage = string.Empty;

            try
            {
                Incidents.Clear();

                var data = _incidentService.GetAll();

                foreach (var incident in data)
                {
                    Incidents.Add(incident);
                }

                _incidentsView =
                    CollectionViewSource.GetDefaultView(Incidents);

                _incidentsView.Filter = FilterIncidents;

                if (Incidents.Count == 0)
                {
                    ErrorMessage = "Инциденты не найдены";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    ex.InnerException?.Message ?? ex.Message;
            }
        }

        [RelayCommand]
        private void Details()
        {
            ErrorMessage = string.Empty;

            if (SelectedIncident == null)
            {
                ErrorMessage = "Выберите инцидент для просмотра";
                return;
            }

            _nav.Navigate<IncidentDetailsPage>(SelectedIncident.IdIncident);
        }

        [RelayCommand]
        private void Add()
        {
            ErrorMessage = string.Empty;

            _nav.Navigate<AddIncidentPage>();
        }

        [RelayCommand]
        private void Update()
        {
            ErrorMessage = string.Empty;

            if (SelectedIncident == null)
            {
                ErrorMessage = "Выберите инцидент для редактирования";
                return;
            }

            _nav.Navigate<AddIncidentPage>(SelectedIncident);
        }

        [RelayCommand]
        private void Delete()
        {
            ErrorMessage = string.Empty;

            if (SelectedIncident == null)
            {
                ErrorMessage =
                    "Сначала выберите инцидент";
                return;
            }

            var result = MessageBox.Show(
                $"Удалить инцидент №{SelectedIncident.IdIncident}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                _incidentService.DeleteIncident(
                    SelectedIncident.IdIncident);

                LoadData();

                ErrorMessage =
                    "Инцидент успешно удалён";
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    ex.InnerException?.Message ?? ex.Message;
            }
        }

        [RelayCommand]
        private void SortBy(string property)
        {
            var view = View;

            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(
                new SortDescription(property,
                    ListSortDirection.Ascending));
        }

        private bool FilterIncidents(object obj)
        {
            if (obj is not Incident incident)
                return false;

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            var search = SearchText.ToLower().Trim();

            return
                (incident.TypeOfIncident?
                    .ToLower()
                    .Contains(search) ?? false)
                ||
                (incident.IdResponseTeamNavigation?
                    .NameTeam?
                    .ToLower()
                    .Contains(search) ?? false);
        }
    }
}