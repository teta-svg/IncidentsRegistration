using IncidentsRegistration.Models;
using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class UserEditPage : Page
    {
        private readonly UserEditViewModel _vm;

        public UserEditPage(UserEditViewModel vm)
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

        public void Initialize(SystemUser? user = null)
        {
            _vm.Initialize(user);
        }
    }
}