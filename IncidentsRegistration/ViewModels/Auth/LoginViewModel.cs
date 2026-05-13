using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Views;

namespace IncidentsRegistration.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly INavigationService _nav;

        public LoginViewModel(
            IAuthService authService,
            IUserService userService,
            INavigationService nav)
        {
            _authService = authService;
            _userService = userService;
            _nav = nav;
        }

        [ObservableProperty]
        private string login = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [RelayCommand]
        private void LoginUser()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage =
                    "Введите логин и пароль";
                return;
            }

            try
            {
                var user =
                    _authService.Login(Login, Password);

                if (user == null)
                {
                    ErrorMessage =
                        "Неверный логин или пароль";
                    return;
                }

                _userService.CurrentUser = user;

                _nav.Navigate<MainAppPage>();
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    $"Ошибка системы: {ex.Message}";
            }
        }

        [RelayCommand]
        private void OpenRegister()
        {
            _nav.Navigate<RegisterPage>();
        }
    }
}