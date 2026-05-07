using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class LoginPage : Page
    {
        private readonly LoginViewModel _vm;
        private readonly RegisterViewModel _registerVm;

        private readonly IIncidentService _incidentService;
        private readonly IExportService _exportService;

        public LoginPage(
            LoginViewModel vm,
            RegisterViewModel registerVm,
            IIncidentService incidentService,
            IExportService exportService)
        {
            InitializeComponent();

            _vm = vm;
            _registerVm = registerVm;
            _incidentService = incidentService;
            _exportService = exportService;

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
                var vm = new MainAppViewModel(user, role, _incidentService, _exportService);

                var mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow?.MainFrame.Navigate(new MainAppPage(vm));
            }
        }
    }
}
