using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class UsersViewModel : ObservableObject
    {
        private readonly IUserService _userService;

        public ObservableCollection<SystemUser> Users { get; } = new();

        public Action? OnAddRequested;
        public Action<SystemUser>? OnUpdateRequested;

        [ObservableProperty]
        private SystemUser? selectedUser;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public UsersViewModel(IUserService userService)
        {
            _userService = userService;
        }

        [RelayCommand]
        public void LoadData()
        {
            ErrorMessage = string.Empty;

            try
            {
                Users.Clear();

                foreach (var user in _userService.GetAllUsers())
                {
                    Users.Add(user);
                }

                if (Users.Count == 0)
                {
                    ErrorMessage = "Пользователи не найдены";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    ex.InnerException?.Message ?? ex.Message;
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

            if (SelectedUser == null)
            {
                ErrorMessage = "Выберите пользователя";
                return;
            }

            OnUpdateRequested?.Invoke(SelectedUser);
        }

        [RelayCommand]
        private void Delete()
        {
            ErrorMessage = string.Empty;

            if (SelectedUser == null)
            {
                ErrorMessage = "Выберите пользователя";
                return;
            }

            var result = MessageBox.Show(
                $"Удалить пользователя {SelectedUser.LoginName}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                _userService.DeleteUser(SelectedUser.IdUser);

                LoadData();

                ErrorMessage = "Пользователь удалён";
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    ex.InnerException?.Message ?? ex.Message;
            }
        }
    }
}