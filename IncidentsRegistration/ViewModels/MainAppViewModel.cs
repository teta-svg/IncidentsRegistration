using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using IncidentsRegistration.Views;

namespace IncidentsRegistration.ViewModels
{
    public partial class MainAppViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IIncidentService _incidentService;
        private readonly IExportService _exportService;
        private readonly IContentNavigationService _contentNav;
        private readonly INavigationService _nav;

        [ObservableProperty]
        private SystemUser? currentUser;

        [ObservableProperty]
        private string role = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public MainAppViewModel(
            IUserService userService,
            IAuthService authService,
            IIncidentService incidentService,
            IExportService exportService, 
            IContentNavigationService contentNav,
            INavigationService nav)
        {
            _userService = userService;
            _authService = authService;
            _incidentService = incidentService;
            _exportService = exportService;
            _contentNav = contentNav;
            _nav = nav;
        }

        public void Initialize()
        {
            CurrentUser = _userService.CurrentUser;

            if (CurrentUser != null)
            {
                Role = _authService
                    .GetRole(CurrentUser)?
                    .Trim()
                    .ToLower() ?? string.Empty;
            }
            else
            {
                Role = string.Empty;
            }

            OnPropertyChanged(nameof(CanSeeOperational));
            OnPropertyChanged(nameof(IsLead));
            OnPropertyChanged(nameof(IsAdmin));
        }

        public bool CanSeeAllIncidents => true;

        public bool CanSeeOperational =>
            Role == "администратор" ||
            Role == "руководитель группы";

        public bool IsLead =>
            Role == "руководитель группы";

        public bool IsAdmin =>
            Role == "администратор";

        [RelayCommand]
        private void ExportGeneralReport()
        {
            ErrorMessage = string.Empty;

            try
            {
                var allIncidents = _incidentService.GetAll();

                if (allIncidents == null || !allIncidents.Any())
                {
                    ErrorMessage =
                        "Нет данных для формирования отчета";
                    return;
                }

                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName =
                        $"Общий_отчет_{DateTime.Now:dd_MM_yyyy}"
                };

                if (sfd.ShowDialog() == true)
                {
                    _exportService.ExportAllIncidentsToExcel(
                        allIncidents,
                        sfd.FileName);

                    ErrorMessage =
                        "Отчет успешно создан";

                    System.Diagnostics.Process.Start(
                        new System.Diagnostics.ProcessStartInfo(
                            sfd.FileName)
                        {
                            UseShellExecute = true
                        });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    $"Ошибка экспорта: {ex.Message}";
            }
        }
        [RelayCommand]
        private void OpenSubjects()
        {
            _contentNav.Navigate<IncidentSubjectsPage>();
        }

        [RelayCommand]
        private void OpenIncidents()
        {
            _contentNav.Navigate<IncidentsPage>();
        }

        [RelayCommand]
        private void OpenActiveIncidents()
        {
            if (CurrentUser == null)
            {
                ErrorMessage = "Пользователь не найден";
                return;
            }

            _contentNav.Navigate<ActiveIncidentsPage>(CurrentUser);
        }

        [RelayCommand]
        private void OpenTeams()
        {
            _contentNav.Navigate<ResponseTeamsPage>();
        }

        [RelayCommand]
        private void OpenUsers()
        {
            _contentNav.Navigate<UsersPage>();
        }


        [RelayCommand]
        private void Logout()
        {
            _nav.Navigate<LoginPage>();
        }
    }
}