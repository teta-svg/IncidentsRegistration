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

            services.AddDbContextFactory<IncidentsDbContext>(options =>
                            options.UseSqlServer("Server=.\\SQLEXPRESS;Database=Incidents_registration;Trusted_Connection=True;TrustServerCertificate=True;"));

            // Сервисы
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IIncidentService, IncidentService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IResponseTeamService, ResponseTeamService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddTransient<IExportService, ExportService>();
            services.AddTransient<IDecisionService, DecisionService>();
            services.AddScoped<IUserService, UserService>();

            // ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<IncidentsViewModel>();
            services.AddTransient<MainAppViewModel>();
            services.AddTransient<AddIncidentViewModel>();
            services.AddTransient<ActiveIncidentsViewModel>();
            services.AddTransient<AddDecisionViewModel>();
            services.AddTransient<AddSubjectViewModel>();
            services.AddTransient<SubjectDetailsViewModel>();
            services.AddTransient<ResponseTeamsViewModel>();
            services.AddTransient<ResponseTeamEditViewModel>();
            services.AddTransient<UsersViewModel>();
            services.AddTransient<UserEditViewModel>();
            services.AddTransient<IncidentDetailsViewModel>();
            services.AddTransient<IncidentSubjectsViewModel>();

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
            services.AddTransient<UsersPage>();
            services.AddTransient<UserEditPage>();
            services.AddTransient<IncidentSubjectsPage>();
            services.AddTransient<IncidentDetailsPage>();

            return services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginPage = Services.GetRequiredService<LoginPage>();
            var mainWindow = new MainWindow();
            mainWindow.MainFrame.Navigate(loginPage);
            mainWindow.Show();
        }
    }
}
