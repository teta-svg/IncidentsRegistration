using IncidentsRegistration.Models;
using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class ResponseTeamEditPage : Page
    {
        private readonly ResponseTeamEditViewModel _vm;

        public ResponseTeamEditPage(ResponseTeamEditViewModel vm)
        {
            InitializeComponent();

            _vm = vm;

            DataContext = _vm;

            _vm.OnRequestGoBack = () =>
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            };
        }

        public void Initialize(ResponseTeam? team = null)
        {
            _vm.Initialize(team);
        }
    }
}