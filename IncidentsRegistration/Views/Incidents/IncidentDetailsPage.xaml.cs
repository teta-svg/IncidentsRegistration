using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class IncidentDetailsPage : Page
    {
        public IncidentDetailsPage(IncidentDetailsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}