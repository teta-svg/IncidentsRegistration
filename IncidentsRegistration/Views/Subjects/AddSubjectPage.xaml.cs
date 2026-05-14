using System.Windows;
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

        private void Inn_LostFocus(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as AddSubjectViewModel;
            var tb = sender as TextBox;

            if (vm == null || tb == null)
                return;

            tb.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

            vm.TryLoadByInn();
        }
    }
}