using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class UsersPage : Page
    {
        private readonly UsersViewModel _vm;

        public UsersPage(UsersViewModel vm)
        {
            InitializeComponent();

            _vm = vm;
            DataContext = _vm;

            Loaded += (s, e) => _vm.LoadData();
        }
    }
}
