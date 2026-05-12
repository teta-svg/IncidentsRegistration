using IncidentsRegistration.Models;
using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class UsersPage : Page
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly UsersViewModel _vm;

        public UsersPage(
            UsersViewModel vm,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _vm = vm;
            _serviceProvider = serviceProvider;

            DataContext = _vm;

            _vm.OnAddRequested = OpenAdd;
            _vm.OnUpdateRequested = OpenUpdate;

            Loaded += (s, e) => _vm.LoadData();
        }

        private void OpenAdd()
        {
            var vm =
                _serviceProvider.GetRequiredService<UserEditViewModel>();

            vm.Initialize();

            var page =
                _serviceProvider.GetRequiredService<UserEditPage>();

            page.Initialize();

            NavigationService.Navigate(page);
        }

        private void OpenUpdate(SystemUser user)
        {
            var vm =
                _serviceProvider.GetRequiredService<UserEditViewModel>();

            vm.Initialize(user);

            var page =
                _serviceProvider.GetRequiredService<UserEditPage>();

            page.Initialize(user);

            NavigationService.Navigate(page);
        }
    }
}