using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using System.Text.RegularExpressions;

namespace IncidentsRegistration.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        public RegisterViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private string errorMessage;

        [RelayCommand]
        private void RegisterUser()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Login))
            {
                ErrorMessage = "Введите логин";
                return;
            }

            var passwordGuideline = @"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^]).{6,}$";

            if (!Regex.IsMatch(Password, passwordGuideline))
            {
                ErrorMessage = "Пароль слишком простой! Нужна 1 заглавная, 1 цифра и символ (!@#$%^)";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Пароли не совпадают";
                return;
            }

            try
            {
                _authService.Register(Login, Password);

                Login = string.Empty;
                Password = string.Empty;
                ConfirmPassword = string.Empty;

                ErrorMessage = "Регистрация успешна!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
        }
    }
}
