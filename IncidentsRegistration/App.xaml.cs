using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Services;
using IncidentsRegistration.ViewModels;
using IncidentsRegistration.Views;
using IncidentsRegistration.Data;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<IncidentsDbContext>(options =>
                options.UseSqlServer("Server=.\\SQLEXPRESS;Database=Incidents_registration;Trusted_Connection=True;TrustServerCertificate=True;"));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IIncidentService, IncidentService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IResponseTeamService, ResponseTeamService>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<IncidentsViewModel>();
            services.AddTransient<MainAppViewModel>();
            services.AddTransient<AddIncidentViewModel>();

            services.AddTransient<LoginPage>();
            services.AddTransient<RegisterPage>();
            services.AddTransient<MainAppPage>();
            services.AddTransient<IncidentsPage>();
            services.AddTransient<AddIncidentPage>();

            return services.BuildServiceProvider();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginPage = ((App)Current).Services.GetRequiredService<LoginPage>();
            var mainWindow = new MainWindow();
            mainWindow.MainFrame.Navigate(loginPage);
            mainWindow.Show();
        }
    }
}
