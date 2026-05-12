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

        [ObservableProperty] private SystemUser _currentUser;
        [ObservableProperty] private string _pageHeader;
        [ObservableProperty] private ObservableCollection<ResponseTeam> _teams;
        [ObservableProperty] private ResponseTeam? _selectedTeam;
        [ObservableProperty] private Visibility _teamSelectionVisibility = Visibility.Collapsed;

        public List<string> Roles { get; } = new() { "администратор", "диспетчер", "руководитель группы" };
        public Action? OnRequestGoBack;

        public UserEditViewModel(IUserService userService, SystemUser? user = null)
        {
            _userService = userService;
            Teams = new ObservableCollection<ResponseTeam>(_userService.GetAllTeams());

            if (user == null)
            {
                CurrentUser = new SystemUser { UserRole = "диспетчер" };
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

                var linkedTeamId = user.SystemUserResponseTeams.FirstOrDefault()?.IdResponseTeam;
                if (linkedTeamId != null)
                {
                    SelectedTeam = Teams.FirstOrDefault(t => t.IdResponseTeam == linkedTeamId);
                }
            }
            UpdateVisibility();
        }

        partial void OnCurrentUserChanged(SystemUser value) => UpdateVisibility();

        [RelayCommand]
        private void Save()
        {
            if (CurrentUser.UserRole == "руководитель группы" && SelectedTeam == null)
            {
                MessageBox.Show("Выберите группу для руководителя");
                return;
            }

            if (CurrentUser.IdUser == 0)
                _userService.AddUser(CurrentUser, SelectedTeam?.IdResponseTeam);
            else
                _userService.UpdateUser(CurrentUser, SelectedTeam?.IdResponseTeam);

            OnRequestGoBack?.Invoke();
        }

        [RelayCommand] private void Back() => OnRequestGoBack?.Invoke();

        private void UpdateVisibility()
        {
            TeamSelectionVisibility = CurrentUser?.UserRole == "руководитель группы"
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        public string SelectedRole
        {
            get => CurrentUser.UserRole;
            set
            {
                CurrentUser.UserRole = value;
                OnPropertyChanged();
                UpdateVisibility();
            }
        }
    }
}
