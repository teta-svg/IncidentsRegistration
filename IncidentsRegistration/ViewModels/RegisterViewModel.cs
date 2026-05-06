using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using System.Windows;

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

        [RelayCommand]
        private void Register()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                MessageBox.Show("Введите логин");
                return;
            }

            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            try
            {
                _authService.Register(Login, Password);

                Login = "";
                Password = "";
                ConfirmPassword = "";

                MessageBox.Show("Регистрация успешна");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
