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

        public ObservableCollection<Incident> Incidents { get; } = new();

        public Action<Incident>? OnShowDetailsRequested;
        public Action? OnAddRequested;
        public Action<Incident>? OnUpdateRequested;

        [ObservableProperty]
        private Incident? selectedIncident;

        public IncidentsViewModel(IIncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        [RelayCommand]
        public void LoadData()
        {
            Incidents.Clear();
            var data = _incidentService.GetAll();
            foreach (var incident in data)
            {
                Incidents.Add(incident);
            }
        }

        [RelayCommand]
        private void Details()
        {
            if (SelectedIncident != null)
            {
                OnShowDetailsRequested?.Invoke(SelectedIncident);
            }
            else
            {
                MessageBox.Show("Выберите инцидент из списка для просмотра деталей.");
            }
        }

        [RelayCommand]
        private void Add()
        {
            OnAddRequested?.Invoke();
        }

        [RelayCommand]
        private void Update()
        {
            if (SelectedIncident != null)
            {
                OnUpdateRequested?.Invoke(SelectedIncident);
            }
            else
            {
                MessageBox.Show("Выберите инцидент для редактирования");
            }
        }

        [RelayCommand]
        private void Delete()
        {
            if (SelectedIncident == null) return;

            var result = MessageBox.Show(
                $"Удалить инцидент №{SelectedIncident.IdIncident} и все связанные с ним решения?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _incidentService.DeleteIncident(SelectedIncident.IdIncident);

                    LoadData();

                    MessageBox.Show("Запись успешно удалена");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось удалить: {ex.Message}");
                }
            }
        }

    }
}
