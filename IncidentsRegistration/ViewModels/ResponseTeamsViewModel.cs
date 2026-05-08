using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class ResponseTeamsViewModel : ObservableObject
    {
        private readonly IResponseTeamService _teamService;
        private List<ResponseTeam> _allTeamsBuffer = new();

        public ObservableCollection<ResponseTeam> ResponseTeams { get; } = new();

        public Action? OnAddRequested;
        public Action<ResponseTeam>? OnUpdateRequested;

        [ObservableProperty]
        private ResponseTeam? selectedResponseTeam;

        [ObservableProperty]
        private string searchText = string.Empty;

        partial void OnSearchTextChanged(string value)
        {
            ApplyFilter();
        }

        public ResponseTeamsViewModel(IResponseTeamService teamService)
        {
            _teamService = teamService;
        }

        [RelayCommand]
        public void LoadData()
        {
            _allTeamsBuffer = _teamService.GetAllTeams();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            ResponseTeams.Clear();

            var filtered = _allTeamsBuffer.Where(t =>
                string.IsNullOrWhiteSpace(SearchText) ||
                t.NameTeam.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                t.DirectorLastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            );

            foreach (var team in filtered)
            {
                ResponseTeams.Add(team);
            }
        }

        [RelayCommand]
        private void Add() => OnAddRequested?.Invoke();

        [RelayCommand]
        private void Update()
        {
            if (SelectedResponseTeam != null)
                OnUpdateRequested?.Invoke(SelectedResponseTeam);
            else
                MessageBox.Show("Выберите группу в списке");
        }

        [RelayCommand]
        private void Delete()
        {
            if (SelectedResponseTeam == null) return;

            if (MessageBox.Show($"Удалить группу «{SelectedResponseTeam.NameTeam}»?", "Удаление",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    _teamService.DeleteTeam(SelectedResponseTeam.IdResponseTeam);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}