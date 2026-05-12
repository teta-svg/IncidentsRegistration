using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class IncidentsViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;
        private readonly IUserService _userService;

        public ObservableCollection<Incident> Incidents { get; } = new();

        public Action<Incident>? OnShowDetailsRequested;
        public Action? OnAddRequested;
        public Action<Incident>? OnUpdateRequested;

        [ObservableProperty]
        private Incident? selectedIncident;

        [ObservableProperty]
        private string role = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public IncidentsViewModel(
            IIncidentService incidentService,
            IUserService userService)
        {
            _incidentService = incidentService;
            _userService = userService;

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
                ErrorMessage =
                    "Выберите инцидент для просмотра";
                return;
            }

            OnShowDetailsRequested?.Invoke(
                SelectedIncident);
        }

        [RelayCommand]
        private void Add()
        {
            ErrorMessage = string.Empty;

            OnAddRequested?.Invoke();
        }

        [RelayCommand]
        private void Update()
        {
            ErrorMessage = string.Empty;

            if (SelectedIncident == null)
            {
                ErrorMessage =
                    "Выберите инцидент для редактирования";
                return;
            }

            OnUpdateRequested?.Invoke(
                SelectedIncident);
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
    }
}