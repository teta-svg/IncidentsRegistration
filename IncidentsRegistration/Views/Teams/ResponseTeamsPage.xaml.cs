using System.Windows;
using System.Windows.Controls;
using IncidentsRegistration.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IncidentsRegistration.Views
{
    public partial class ResponseTeamsPage : Page
    {
        private readonly ResponseTeamsViewModel _viewModel;

        public ResponseTeamsPage()
        {
            InitializeComponent();

            var viewModel = ((App)Application.Current).Services.GetRequiredService<ResponseTeamsViewModel>();

            viewModel.OnAddRequested = () =>
            {
                this.NavigationService.Navigate(new ResponseTeamEditPage(null));
            };

            viewModel.OnUpdateRequested = (team) =>
            {
                this.NavigationService.Navigate(new ResponseTeamEditPage(team));
            };

            this.Loaded += (s, e) => viewModel.LoadData();

            DataContext = viewModel;
            viewModel.LoadData();
        }

    }
}
