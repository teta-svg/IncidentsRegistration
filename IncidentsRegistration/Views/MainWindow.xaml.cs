using IncidentsRegistration.Data;
using IncidentsRegistration.Services;
using IncidentsRegistration.ViewModels;
using System.Windows;

namespace IncidentsRegistration.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var dbContext = new IncidentsDbContext();

            var authService = new AuthService(dbContext);
            var incidentService = new IncidentService(dbContext);

            var exportService = new ExportService();

            MainFrame.Navigate(
                new LoginPage(
                    new LoginViewModel(authService),
                    new RegisterViewModel(authService),
                    incidentService,
                    exportService
                )
            );
        }
    }
}
