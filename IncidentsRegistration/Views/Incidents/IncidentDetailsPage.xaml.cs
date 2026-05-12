using IncidentsRegistration.ViewModels;
using System.Windows;
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
