using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace IncidentsRegistration.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var loginPage = ((App)Application.Current)
                .Services.GetRequiredService<LoginPage>();

            MainFrame.Navigate(loginPage);
        }
    }
}
