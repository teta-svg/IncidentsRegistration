using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;

namespace IncidentsRegistration.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public LoginViewModel(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string errorMessage;

        public Action<SystemUser>? OnLoginSuccess;

        [RelayCommand]
        private void LoginUser()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Введите логин и пароль";
                return;
            }

            try
            {
                var user = _authService.Login(Login, Password);

                if (user == null)
                {
                    ErrorMessage = "Неверный логин или пароль";
                    return;
                }
                _userService.CurrentUser = user;

                OnLoginSuccess?.Invoke(user);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка системы: {ex.Message}";
            }
        }
    }
}
