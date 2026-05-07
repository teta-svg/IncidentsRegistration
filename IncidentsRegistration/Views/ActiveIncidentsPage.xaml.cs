using IncidentsRegistration.ViewModels;
using IncidentsRegistration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using System.Windows;

namespace IncidentsRegistration.Views
{
    public partial class ActiveIncidentsPage : Page
    {
        public ActiveIncidentsPage(ActiveIncidentsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            var services = ((App)Application.Current).Services;

            vm.OnMakeDecisionRequested = (incidentId) =>
            {
                var decisionService = services.GetRequiredService<IDecisionService>();

                var decisionVm = new AddDecisionViewModel(decisionService, incidentId);

                var decisionPage = new AddDecisionPage(decisionVm);

                NavigationService.Navigate(decisionPage);
            };
            this.Loaded += (s, e) => vm.LoadDataCommand.Execute(null);
        }
    }
}
