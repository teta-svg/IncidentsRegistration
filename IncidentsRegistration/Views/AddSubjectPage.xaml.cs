using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class AddSubjectPage : Page
    {
        public AddSubjectPage(AddSubjectViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
