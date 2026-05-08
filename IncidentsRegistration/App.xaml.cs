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

            // Сервисы
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IIncidentService, IncidentService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IResponseTeamService, ResponseTeamService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddTransient<IExportService, ExportService>();
            services.AddTransient<IDecisionService, DecisionService>();

            // ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<IncidentsViewModel>();
            services.AddTransient<MainAppViewModel>();
            services.AddTransient<AddIncidentViewModel>();
            services.AddTransient<ActiveIncidentsViewModel>();
            services.AddTransient<AddDecisionViewModel>();
            services.AddTransient<IncidentSubjectsViewModel>();
            services.AddTransient<AddSubjectViewModel>();
            services.AddTransient<SubjectDetailsViewModel>();
            services.AddTransient<ResponseTeamsViewModel>();
            services.AddTransient<ResponseTeamEditViewModel>();

            // Страницы
            services.AddTransient<LoginPage>();
            services.AddTransient<RegisterPage>();
            services.AddTransient<MainAppPage>();
            services.AddTransient<IncidentsPage>();
            services.AddTransient<AddIncidentPage>();
            services.AddTransient<ActiveIncidentsPage>();
            services.AddTransient<AddDecisionPage>();
            services.AddTransient<IncidentSubjectsPage>();
            services.AddTransient<AddSubjectPage>();
            services.AddTransient<SubjectDetailsPage>();
            services.AddTransient<ResponseTeamsPage>();
            services.AddTransient<ResponseTeamEditPage>();

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
