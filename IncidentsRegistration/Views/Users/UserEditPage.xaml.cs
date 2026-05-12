using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class UserEditPage : Page
    {
        public UserEditPage(SystemUser? user)
        {
            InitializeComponent();

            var userService = ((App)Application.Current).Services.GetRequiredService<IUserService>();
            var viewModel = new UserEditViewModel(userService, user);

            viewModel.OnRequestGoBack = () =>
            {
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            };

            DataContext = viewModel;
        }

    }
}
