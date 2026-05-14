using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class UserEditPage : Page
    {
        public UserEditPage(UserEditViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
