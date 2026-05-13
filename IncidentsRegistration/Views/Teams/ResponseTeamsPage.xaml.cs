using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class ResponseTeamsPage : Page
    {
        public ResponseTeamsPage(ResponseTeamsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            Loaded += (s, e) => vm.LoadData();
        }
    }
}