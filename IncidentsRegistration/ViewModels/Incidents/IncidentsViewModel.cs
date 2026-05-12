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
        private string role;

        [ObservableProperty]
        private string errorMessage;

        public IncidentsViewModel(
            IIncidentService incidentService,
            IUserService userService)
        {
            _incidentService = incidentService;
            _userService = userService;

            if (_userService.CurrentUser != null)
            {
                Role = _userService.CurrentUser.UserRole?.ToLower().Trim();
            }
        }

        public bool CanEdit => Role == "администратор" || Role == "руководитель группы";
        public bool CanDelete => Role == "администратор";


        [RelayCommand]
        public void LoadData()
        {
            Incidents.Clear();
            var data = _incidentService.GetAll();
            foreach (var incident in data)
                Incidents.Add(incident);
        }

        [RelayCommand]
        private void Details()
        {
            if (SelectedIncident != null)
                OnShowDetailsRequested?.Invoke(SelectedIncident);
            else
                MessageBox.Show("Выберите инцидент из списка для просмотра деталей.");
        }

        [RelayCommand]
        private void Add() =>
            OnAddRequested?.Invoke();

        [RelayCommand]
        private void Update()
        {
            if (SelectedIncident != null)
                OnUpdateRequested?.Invoke(SelectedIncident);
            else
                MessageBox.Show("Выберите инцидент для редактирования");
        }

        [RelayCommand]
        private void Delete()
        {
            if (SelectedIncident == null)
            {
                ErrorMessage = "Сначала выберите инцидент!";
                return;
            }

            var result = MessageBox.Show(
                $"Удалить инцидент №{SelectedIncident.IdIncident}?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _incidentService.DeleteIncident(SelectedIncident.IdIncident);
                    LoadData();
                    ErrorMessage = "Запись удалена успешно";
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Не удалось удалить: {ex.Message}";
                }
            }
        }
    }
}
