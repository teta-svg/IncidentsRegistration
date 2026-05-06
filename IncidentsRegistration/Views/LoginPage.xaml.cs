using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using IncidentsRegistration.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace IncidentsRegistration.Views
{
    public partial class LoginPage : Page
    {
        private readonly LoginViewModel _vm;
        private readonly RegisterViewModel _registerVm;

        public LoginPage(LoginViewModel vm, RegisterViewModel registerVm)
        {
            InitializeComponent();

            _vm = vm;
            _registerVm = registerVm;
            DataContext = _vm;

            _vm.OnLoginSuccess = OnLoginSuccess;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            _vm.Password = PasswordBox.Password;
            _vm.LoginUserCommand.Execute(null);
        }

        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegisterPage(_registerVm));
        }

        private void OnLoginSuccess(SystemUser user)
        {
            var services = ((App)Application.Current).Services;
            var authService = services.GetService<IAuthService>();

            if (authService != null)
            {
                var role = authService.GetRole(user);
                var vm = new MainAppViewModel(user, role);

                var mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow?.MainFrame.Navigate(new MainAppPage(vm));
            }
        }

    }
}
