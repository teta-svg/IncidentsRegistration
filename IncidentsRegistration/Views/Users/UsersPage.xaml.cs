using System.Windows;
using System.Windows.Controls;
using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IncidentsRegistration.Views
{
    public partial class UsersPage : Page
    {
        private readonly UsersViewModel _viewModel;

        public UsersPage()
        {
            InitializeComponent();
            _viewModel = ((App)Application.Current).Services.GetRequiredService<UsersViewModel>();

            _viewModel.OnAddRequested = () =>
            {
                NavigationService.Navigate(new UserEditPage(null));
            };

            _viewModel.OnUpdateRequested = (user) =>
            {
                NavigationService.Navigate(new UserEditPage(user));
            };
            this.Loaded += (s, e) => _viewModel.LoadData();

            DataContext = _viewModel;
        }
    }
}
