using IncidentsRegistration.Models;
using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class ActiveIncidentsPage : Page
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ActiveIncidentsViewModel _vm;

        public ActiveIncidentsPage(ActiveIncidentsViewModel vm, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
            _serviceProvider = serviceProvider;

            _vm.OnMakeDecisionRequested = (incidentId) =>
            {
                var decisionPage = _serviceProvider.GetRequiredService<AddDecisionPage>();

                if (decisionPage.DataContext is AddDecisionViewModel dVm)
                {
                    dVm.Initialize(incidentId, _vm.CurrentUser);
                }

                NavigationService.Navigate(decisionPage);
            };
        }

        public void Initialize(SystemUser user)
        {
            _vm.Initialize(user);
        }
    }
}