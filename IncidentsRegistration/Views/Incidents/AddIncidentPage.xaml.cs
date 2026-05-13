using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class AddIncidentPage : Page
    {
        public AddIncidentPage(AddIncidentViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }
    }
}