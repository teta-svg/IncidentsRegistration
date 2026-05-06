using IncidentsRegistration.Models;

namespace IncidentsRegistration.Interfaces
{
    public interface IIncidentService
    {
        void CreateIncident(Incident incident);
        List<Incident> GetAll();
        Incident? GetById(int id);
        List<Incident> GetActiveIncidentsByTeam(int responseTeamId);
    }
}
