using IncidentsRegistration.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class RegisterPage : Page
    {
        private readonly RegisterViewModel _vm;

        public RegisterPage(RegisterViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            _vm.Password = PasswordBox.Password;
            _vm.ConfirmPassword = ConfirmPasswordBox.Password;

            if (_vm.RegisterCommand.CanExecute(null))
                _vm.RegisterCommand.Execute(null);
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}