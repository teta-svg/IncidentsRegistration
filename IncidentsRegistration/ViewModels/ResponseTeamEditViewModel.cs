using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class ResponseTeamEditViewModel : ObservableObject
    {
        private readonly IResponseTeamService _teamService;

        [ObservableProperty]
        private ResponseTeam _currentTeam;

        [ObservableProperty]
        private string _pageHeader;

        public Action? OnRequestGoBack;

        public ResponseTeamEditViewModel(IResponseTeamService teamService, ResponseTeam? team = null)
        {
            _teamService = teamService;

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
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentTeam.NameTeam))
                {
                    MessageBox.Show("Введите название команды");
                    return;
                }

                if (CurrentTeam.IdResponseTeam == 0)
                {
                    _teamService.AddTeam(CurrentTeam);
                    MessageBox.Show("Группа успешно добавлена");
                }
                else
                {
                    _teamService.UpdateTeam(CurrentTeam);
                    MessageBox.Show("Данные обновлены");
                }

                OnRequestGoBack?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        [RelayCommand]
        private void Back()
        {
            OnRequestGoBack?.Invoke();
        }
    }
}
