using IncidentsRegistration.Models;

namespace IncidentsRegistration.Interfaces
{
    public interface ILocationService
    {
        Location GetOrCreateLocation(Location location);
    }
}
