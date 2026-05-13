using IncidentsRegistration.Interfaces;
using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class MainAppPage : Page
    {
        public MainAppPage(
            MainAppViewModel vm,
            IContentNavigationService contentNav)
        {
            InitializeComponent();

            DataContext = vm;

            contentNav.SetFrame(ContentFrame);

            contentNav.Navigate<IncidentsPage>();

            vm.Initialize();
        }
    }
}