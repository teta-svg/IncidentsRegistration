using IncidentsRegistration.Data;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;

namespace IncidentsRegistration.Services
{
    public class LocationService : ILocationService
    {
        private readonly IncidentsDbContext _context;

        public LocationService(IncidentsDbContext context)
        {
            _context = context;
        }

        public void AddLocationToIncident(int incidentId, Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            var incident = _context.Incidents.Find(incidentId);

            if (incident == null)
                throw new ArgumentNullException(nameof(incident));

            var incidentLocation = new IncidentLocation
            {
                IdIncident = incidentId,
                IdLocationNavigation = location,
            };

            _context.IncidentLocations.Add(incidentLocation);
            _context.SaveChanges();

        }

        public void AttachToIncident(int incidentId, int locationId)
        {
            var incident = _context.Incidents.Find(incidentId);
            var location = _context.Locations.Find(locationId);

            if (incident == null)
                throw new ArgumentNullException(nameof(incident));

            if (location == null)
                throw new ArgumentNullException(nameof(location));

            var exists = _context.IncidentLocations
                .Any(il => il.IdIncident == incidentId && il.IdLocation == locationId);

            if (exists)
                throw new ArgumentException("Связь уже существует");

            var incidentLocation = new IncidentLocation
            {
                IdIncident = incidentId,
                IdLocation = locationId
            };

            _context.IncidentLocations.Add(incidentLocation);
            _context.SaveChanges();
        }

    }
}
