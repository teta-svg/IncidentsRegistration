using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class SubjectDetailsPage : Page
    {
        public SubjectDetailsPage(SubjectDetailsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}