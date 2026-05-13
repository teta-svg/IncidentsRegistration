using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class AddDecisionPage : Page
    {
        public AddDecisionPage(AddDecisionViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
