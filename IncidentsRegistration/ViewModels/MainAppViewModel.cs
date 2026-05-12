using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using System.Windows;

namespace IncidentsRegistration.ViewModels
{
    public partial class MainAppViewModel : ObservableObject
    {
        private readonly IIncidentService _incidentService;
        private readonly IExportService _exportService; 

        [ObservableProperty]
        private SystemUser _currentUser;

        [ObservableProperty]
        private string _role;

        public MainAppViewModel(
            IUserService userService,
            IAuthService authService,
            IIncidentService incidentService,
            IExportService exportService)
        {
            _incidentService = incidentService;
            _exportService = exportService;

            CurrentUser = userService.CurrentUser;

            if (CurrentUser != null)
            {
                Role = authService.GetRole(CurrentUser)?.Trim().ToLower();
            }
        }

        public bool CanSeeAllIncidents => true;
        public bool CanSeeOperational => Role == "администратор" || Role == "руководитель группы";
        public bool IsLead => Role == "руководитель группы";
        public bool IsAdmin => Role == "администратор";

        [RelayCommand]
        private void ExportGeneralReport()
        {
            try
            {
                var allIncidents = _incidentService.GetAll();

                if (allIncidents == null || !allIncidents.Any())
                {
                    MessageBox.Show("Нет данных для формирования отчета.");
                    return;
                }

                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = $"Общий_отчет_{DateTime.Now:dd_MM_yyyy}"
                };

                if (sfd.ShowDialog() == true)
                {
                    _exportService.ExportAllIncidentsToExcel(allIncidents, sfd.FileName);

                    MessageBox.Show("Отчет успешно создан!");

                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName)
                    {
                        UseShellExecute = true
                    });//открыть файл после сохранения
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта: {ex.Message}");
            }
        }
    }
}
