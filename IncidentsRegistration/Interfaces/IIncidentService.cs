using IncidentsRegistration.Models;

namespace IncidentsRegistration.Interfaces
{
    public interface IIncidentService
    {
        void CreateIncident(Incident incident, int locationId);
        List<Incident> GetAll();
        Incident? GetById(int id);
        List<Incident> GetActiveIncidentsByTeam(int responseTeamId);
        List<Incident> GetIncidentsByTeam(int responseTeamId);
        Incident? GetFullIncidentDetails(int incidentId);
        void UpdateIncident(Incident incident, Location location);
        void DeleteIncident(int incidentId);
    }
}
