using System.Windows;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class AddSubjectPage : Page
    {
        public AddSubjectPage(AddSubjectViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService?.CanGoBack == true)
                this.NavigationService.GoBack();
        }
    }
}