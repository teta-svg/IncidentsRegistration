using IncidentsRegistration.Data;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Services
{
    public class LocationService : ILocationService
    {
        private readonly IncidentsDbContext _context;

        public LocationService(IncidentsDbContext context)
        {
            _context = context;
        }

        public Location GetOrCreateLocation(Location location)
        {
            var existing = _context.Locations
                .AsNoTracking()
                .FirstOrDefault(l =>
                    l.Settlement == location.Settlement &&
                    l.Street == location.Street &&
                    l.House == location.House &&
                    l.Room == location.Room);

            if (existing != null) return existing;

            _context.Locations.Add(location);
            _context.SaveChanges();
            return location;
        }
    }

}
