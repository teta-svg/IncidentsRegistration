using IncidentsRegistration.Models;
using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class ResponseTeamsPage : Page
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ResponseTeamsViewModel _vm;

        public ResponseTeamsPage(
            ResponseTeamsViewModel vm,
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
                _serviceProvider.GetRequiredService<ResponseTeamEditViewModel>();

            vm.Initialize();

            var page =
                _serviceProvider.GetRequiredService<ResponseTeamEditPage>();

            page.Initialize();

            NavigationService.Navigate(page);
        }

        private void OpenUpdate(ResponseTeam team)
        {
            var vm =
                _serviceProvider.GetRequiredService<ResponseTeamEditViewModel>();

            vm.Initialize(team);

            var page =
                _serviceProvider.GetRequiredService<ResponseTeamEditPage>();

            page.Initialize(team);

            NavigationService.Navigate(page);
        }
    }
}