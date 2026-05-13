using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class IncidentsPage : Page
    {
        public IncidentsPage(IncidentsViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;

            Loaded += (s, e) => vm.LoadData();
        }
    }
}