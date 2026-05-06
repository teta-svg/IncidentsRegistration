using IncidentsRegistration.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace IncidentsRegistration.Views
{
    public partial class AddIncidentPage : Page
    {
        private readonly AddIncidentViewModel _vm;

        public AddIncidentPage(AddIncidentViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _vm.SaveCommand.Execute(null);

            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }
    }
}
