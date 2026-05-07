using IncidentsRegistration.Models;

namespace IncidentsRegistration.Interfaces
{
    public interface IExportService
    {
        void ExportAllIncidentsToExcel(List<Incident> incidents, string filePath);
    }
}
