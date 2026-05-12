using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class MainAppPage : Page
    {
        private readonly MainAppViewModel _vm;

        public MainAppPage(MainAppViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            Loaded += (s, e) =>
            {
                _vm.Initialize();
            };
        }
    }
}