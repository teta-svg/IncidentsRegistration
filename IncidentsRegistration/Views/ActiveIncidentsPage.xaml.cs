using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class ActiveIncidentsPage : Page
    {
        public ActiveIncidentsPage(ActiveIncidentsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

    }
}
