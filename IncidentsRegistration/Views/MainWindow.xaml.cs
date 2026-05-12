using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace IncidentsRegistration.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var scope = ((App)Application.Current).Services;
            var loginPage = scope.GetRequiredService<LoginPage>();

            MainFrame.Navigate(loginPage);
        }
    }
}
