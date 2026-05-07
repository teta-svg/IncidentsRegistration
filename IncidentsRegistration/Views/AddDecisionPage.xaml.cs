using IncidentsRegistration.ViewModels;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace IncidentsRegistration.Views
{
    public partial class AddDecisionPage : Page
    {
        public AddDecisionPage(AddDecisionViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;

            vm.OnSuccess = () =>
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            };
        }
    }
}
