using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Views;
using System.Text.RegularExpressions;

namespace IncidentsRegistration.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _nav;

        public RegisterViewModel(
            IAuthService authService,
            INavigationService nav)
        {
            _authService = authService;
            _nav = nav;
        }

        [ObservableProperty]
        private string login = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [RelayCommand]
        private void RegisterUser()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Login))
            {
                ErrorMessage = "Введите логин";
                return;
            }

            var passwordGuideline =
                @"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^]).{6,}$";

            if (!Regex.IsMatch(
                    Password,
                    passwordGuideline))
            {
                ErrorMessage =
                    "Пароль слишком простой!";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage =
                    "Пароли не совпадают";
                return;
            }

            try
            {
                _authService.Register(Login, Password);

                Login = string.Empty;
                Password = string.Empty;
                ConfirmPassword = string.Empty;

                ErrorMessage =
                    "Регистрация успешна!";
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    $"Ошибка: {ex.Message}";
            }
        }

        [RelayCommand]
        private void BackToLogin()
        {
            _nav.Navigate<LoginPage>();
        }
    }
}