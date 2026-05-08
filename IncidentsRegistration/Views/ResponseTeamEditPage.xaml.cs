using System.Windows.Controls;
using IncidentsRegistration.Data;
using IncidentsRegistration.Models;
using IncidentsRegistration.Services;
using IncidentsRegistration.ViewModels;

namespace IncidentsRegistration.Views
{
    public partial class ResponseTeamEditPage : Page
    {
        public ResponseTeamEditPage(ResponseTeam? team)
        {
            InitializeComponent();

            var context = new IncidentsDbContext();
            var service = new ResponseTeamService(context);
            var viewModel = new ResponseTeamEditViewModel(service, team);

            viewModel.OnRequestGoBack = () =>
            {
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            };

            DataContext = viewModel;
        }
    }
}