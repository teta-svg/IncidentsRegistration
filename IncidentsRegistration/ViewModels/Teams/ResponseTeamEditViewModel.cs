using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;

namespace IncidentsRegistration.ViewModels
{
    public partial class ResponseTeamEditViewModel : ObservableObject
    {
        private readonly IResponseTeamService _teamService;

        [ObservableProperty]
        private ResponseTeam currentTeam = new();

        [ObservableProperty]
        private string pageHeader = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public Action? OnRequestGoBack;

        public ResponseTeamEditViewModel(IResponseTeamService teamService)
        {
            _teamService = teamService;
        }

        public void Initialize(ResponseTeam? team = null)
        {
            ErrorMessage = string.Empty;

            if (team == null)
            {
                CurrentTeam = new ResponseTeam();
                PageHeader = "Новая группа";
            }
            else
            {
                CurrentTeam = new ResponseTeam
                {
                    IdResponseTeam = team.IdResponseTeam,
                    NameTeam = team.NameTeam,
                    DirectorLastName = team.DirectorLastName,
                    DirectorFirstName = team.DirectorFirstName,
                    DirectorPatronymic = team.DirectorPatronymic,
                    NumberOfPeople = team.NumberOfPeople
                };

                PageHeader = "Редактирование";
            }
        }

        [RelayCommand]
        private void Save()
        {
            ErrorMessage = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(CurrentTeam.NameTeam))
                {
                    ErrorMessage = "Введите название команды";
                    return;
                }

                if (CurrentTeam.IdResponseTeam == 0)
                {
                    _teamService.AddTeam(CurrentTeam);
                    ErrorMessage = "Группа успешно добавлена";
                }
                else
                {
                    _teamService.UpdateTeam(CurrentTeam);
                    ErrorMessage = "Данные обновлены";
                }

                OnRequestGoBack?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerException?.Message ?? ex.Message;
            }
        }

        [RelayCommand]
        private void Back()
        {
            OnRequestGoBack?.Invoke();
        }
    }
}