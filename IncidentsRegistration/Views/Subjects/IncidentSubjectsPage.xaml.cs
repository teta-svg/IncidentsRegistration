using IncidentsRegistration.ViewModels;
using System.Windows.Controls;

namespace IncidentsRegistration.Views
{
    public partial class IncidentSubjectsPage : Page
    {
        public IncidentSubjectsPage(IncidentSubjectsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            Loaded += (s, e) => vm.LoadData();
        }
    }
}