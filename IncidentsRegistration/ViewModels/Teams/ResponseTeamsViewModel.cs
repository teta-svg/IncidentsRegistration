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

        [ObservableProperty]
        private string errorMessage = string.Empty;

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
            ErrorMessage = string.Empty;

            try
            {
                _allTeamsBuffer = _teamService.GetAllTeams();

                ApplyFilter();

                if (ResponseTeams.Count == 0)
                {
                    ErrorMessage = "Группы не найдены";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    ex.InnerException?.Message ?? ex.Message;
            }
        }

        private void ApplyFilter()
        {
            ResponseTeams.Clear();

            var filtered = _allTeamsBuffer.Where(t =>
                string.IsNullOrWhiteSpace(SearchText) ||

                (t.NameTeam?.Contains(
                    SearchText,
                    StringComparison.OrdinalIgnoreCase) ?? false)

                ||

                (t.DirectorLastName?.Contains(
                    SearchText,
                    StringComparison.OrdinalIgnoreCase) ?? false)
            );

            foreach (var team in filtered)
            {
                ResponseTeams.Add(team);
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
            ErrorMessage = string.Empty;

            if (SelectedResponseTeam == null)
            {
                ErrorMessage = "Выберите группу";
                return;
            }

            OnUpdateRequested?.Invoke(SelectedResponseTeam);
        }

        [RelayCommand]
        private void Delete()
        {
            ErrorMessage = string.Empty;

            if (SelectedResponseTeam == null)
            {
                ErrorMessage = "Выберите группу";
                return;
            }

            var result = MessageBox.Show(
                $"Удалить группу «{SelectedResponseTeam.NameTeam}»?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                _teamService.DeleteTeam(
                    SelectedResponseTeam.IdResponseTeam);

                LoadData();

                ErrorMessage = "Группа удалена";
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    ex.InnerException?.Message ?? ex.Message;
            }
        }
    }
}