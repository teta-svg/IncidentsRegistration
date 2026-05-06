using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        public Action<SystemUser>? OnLoginSuccess;

        [RelayCommand]
        private void LoginUser()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                MessageBox.Show("Введите логин");
                return;
            }

            var user = _authService.Login(Login, Password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            OnLoginSuccess?.Invoke(user);
        }
    }
}
