using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class ResponseTeamEditPage : Page
    {
        public ResponseTeamEditPage(ResponseTeamEditViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}