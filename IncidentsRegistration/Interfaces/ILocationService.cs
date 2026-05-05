using IncidentsRegistration.Models;

namespace IncidentsRegistration.Interfaces
{
    public interface ILocationService
    {
        void AddLocationToIncident(int incidentId, Location location);
        void AttachToIncident(int incidentId, int locationId);
    }
}
