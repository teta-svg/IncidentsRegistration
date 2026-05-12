using IncidentsRegistration.ViewModels;
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

            //Авто-возврат при успехе
            vm.OnSuccess = () =>
            {
                if (NavigationService.CanGoBack) NavigationService.GoBack();
            };
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
