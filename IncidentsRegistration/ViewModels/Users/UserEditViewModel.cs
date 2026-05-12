using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class UserEditViewModel : ObservableObject
    {
        private readonly IUserService _userService;

        [ObservableProperty]
        private SystemUser currentUser = new();

        [ObservableProperty]
        private string pageHeader = string.Empty;

        [ObservableProperty]
        private ObservableCollection<ResponseTeam> teams = new();

        [ObservableProperty]
        private ResponseTeam? selectedTeam;

        [ObservableProperty]
        private Visibility teamSelectionVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private string selectedRole = string.Empty;

        public List<string> Roles { get; } = new()
        {
            "администратор",
            "диспетчер",
            "руководитель группы"
        };

        public Action? OnRequestGoBack;

        public UserEditViewModel(IUserService userService)
        {
            _userService = userService;

            Teams = new ObservableCollection<ResponseTeam>(
                _userService.GetAllTeams());
        }

        public void Initialize(SystemUser? user = null)
        {
            ErrorMessage = string.Empty;

            if (user == null)
            {
                CurrentUser = new SystemUser
                {
                    UserRole = "диспетчер"
                };

                PageHeader = "Новый пользователь";
            }
            else
            {
                CurrentUser = new SystemUser
                {
                    IdUser = user.IdUser,
                    LoginName = user.LoginName,
                    UserPassword = user.UserPassword,
                    UserRole = user.UserRole
                };

                PageHeader = "Редактирование";

                var linkedTeamId = user
                    .SystemUserResponseTeams
                    .FirstOrDefault()
                    ?.IdResponseTeam;

                if (linkedTeamId != null)
                {
                    SelectedTeam = Teams.FirstOrDefault(
                        t => t.IdResponseTeam == linkedTeamId);
                }
            }

            SelectedRole = CurrentUser.UserRole;

            UpdateVisibility();
        }

        partial void OnSelectedRoleChanged(string value)
        {
            CurrentUser.UserRole = value;
            UpdateVisibility();
        }

        [RelayCommand]
        private void Save()
        {
            ErrorMessage = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(CurrentUser.LoginName))
                {
                    ErrorMessage = "Введите логин";
                    return;
                }

                if (string.IsNullOrWhiteSpace(CurrentUser.UserPassword))
                {
                    ErrorMessage = "Введите пароль";
                    return;
                }

                if (CurrentUser.UserRole == "руководитель группы"
                    && SelectedTeam == null)
                {
                    ErrorMessage = "Выберите группу";
                    return;
                }

                if (CurrentUser.IdUser == 0)
                {
                    _userService.AddUser(
                        CurrentUser,
                        SelectedTeam?.IdResponseTeam);

                    ErrorMessage = "Пользователь создан";
                }
                else
                {
                    _userService.UpdateUser(
                        CurrentUser,
                        SelectedTeam?.IdResponseTeam);

                    ErrorMessage = "Данные обновлены";
                }

                OnRequestGoBack?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    ex.InnerException?.Message ?? ex.Message;
            }
        }

        [RelayCommand]
        private void Back()
        {
            OnRequestGoBack?.Invoke();
        }

        private void UpdateVisibility()
        {
            TeamSelectionVisibility =
                CurrentUser.UserRole == "руководитель группы"
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }
    }
}