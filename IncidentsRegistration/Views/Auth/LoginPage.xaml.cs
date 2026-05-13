using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class LoginPage : Page
    {
        private readonly LoginViewModel _vm;

        public LoginPage(LoginViewModel vm)
        {
            InitializeComponent();

            _vm = vm;

            DataContext = _vm;
        }

        private void Login_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _vm.Password = PasswordBox.Password;

            _vm.LoginUserCommand.Execute(null);
        }
    }
}