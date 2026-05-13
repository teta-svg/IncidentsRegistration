using System.Windows;
using IncidentsRegistration.Interfaces;

namespace IncidentsRegistration.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(INavigationService navigation)
        {
            InitializeComponent();

            navigation.SetFrame(MainFrame);

            navigation.Navigate<LoginPage>();
        }
    }
}