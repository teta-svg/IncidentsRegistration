using IncidentsRegistration.Models;
using IncidentsRegistration.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class LoginPage : Page
    {
        private readonly LoginViewModel _vm;
        private readonly RegisterPage _registerPage;
        private readonly MainAppPage _mainPage;

        public LoginPage(LoginViewModel vm, RegisterPage registerPage, MainAppPage mainPage)
        {
            InitializeComponent();

            _vm = vm;
            _registerPage = registerPage;
            _mainPage = mainPage;

            DataContext = _vm;//связь с вью моделью

            _vm.OnLoginSuccess = OnLoginSuccess;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            _vm.Password = PasswordBox.Password;
            _vm.LoginUserCommand.Execute(null);
        }

        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(_registerPage);
        }

        private void OnLoginSuccess(SystemUser user)
        {
            NavigationService.Navigate(_mainPage);
        }
    }
}