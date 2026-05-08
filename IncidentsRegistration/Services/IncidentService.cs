using IncidentsRegistration.Data;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Services
{
    public class IncidentService : IIncidentService
    {
        private readonly IncidentsDbContext _context;

        public IncidentService(IncidentsDbContext context)
        {
            _context = context;
        }

        public void CreateIncident(Incident incident, int locationId)
        {
            _context.ChangeTracker.Clear();

            incident.IncidentLocations.Add(new IncidentLocation
            {
                IdLocation = locationId
            });

            _context.Incidents.Add(incident);
            _context.SaveChanges();
        }


        public List<Incident> GetAll() => BaseQuery().ToList();
        public Incident? GetById(int id) => BaseQuery().FirstOrDefault(i=>i.IdIncident == id);

        private IQueryable<Incident> BaseQuery()
        {
            return _context.Incidents
                .AsNoTracking()
                .Include(i => i.Decision)
                    .ThenInclude(d => d.CriminalCase)
                .Include(i => i.Decision)
                    .ThenInclude(d => d.TerritorialTransfer)
                .Include(i => i.IdResponseTeamNavigation)
                .Include(i => i.IncidentLocations)
                    .ThenInclude(il => il.IdLocationNavigation)
                .Include(i => i.SubjectRoles)
                    .ThenInclude(sr => sr.IdSubjectNavigation)
                .OrderByDescending(i => i.RegistrationDate)
                .ThenByDescending(i => i.RegistrationTime);
        }


        public List<Incident> GetActiveIncidentsByTeam(int responseTeamId)
        {
            return BaseQuery()
                .Where(i => i.Decision == null)
                .Where(i => i.IdResponseTeam == responseTeamId)
                .ToList();
        }

        public Incident? GetFullIncidentDetails(int incidentId)
        {
            var incident = _context.Incidents
                .Include(i => i.IdResponseTeamNavigation)
                .Include(i => i.IncidentLocations)
                    .ThenInclude(il => il.IdLocationNavigation)
                .Include(i => i.SubjectRoles)
                    .ThenInclude(sr => sr.IdSubjectNavigation)
                .Include(i => i.Decision)
                    .ThenInclude(d => d.CriminalCase)
                .Include(i => i.Decision)
                    .ThenInclude(d => d.TerritorialTransfer)
                .FirstOrDefault(i => i.IdIncident == incidentId);

            if (incident != null && incident.SubjectRoles != null)
            {
                incident.SubjectRoles = incident.SubjectRoles
                    .GroupBy(sr => sr.IdSubject)
                    .Select(g => g
                        .OrderByDescending(sr => sr.DateOfInvolvement)
                        .ThenByDescending(sr => sr.IdSubjectRole)
                        .First())
                    .ToList();
            }

            return incident;
        }


        public void UpdateIncident(Incident incident, Location location)
        {
            _context.ChangeTracker.Clear();

            _context.Entry(incident).Property(i => i.IdResponseTeam).IsModified = true;

            _context.Incidents.Update(incident);
            _context.Locations.Update(location);

            _context.SaveChanges();
        }
        public void DeleteIncident(int incidentId)
        {
            var incident = _context.Incidents
                .Include(i => i.IncidentLocations)
                .Include(i => i.SubjectRoles)
                .Include(i => i.Decision)
                    .ThenInclude(d => d.CriminalCase)
                .Include(i => i.Decision)
                    .ThenInclude(d => d.TerritorialTransfer)
                .FirstOrDefault(i => i.IdIncident == incidentId);

            if (incident == null) return;

            if (incident.IncidentLocations.Any())
            {
                _context.IncidentLocations.RemoveRange(incident.IncidentLocations);
            }

            if (incident.SubjectRoles.Any())
            {
                _context.SubjectRoles.RemoveRange(incident.SubjectRoles);
            }

            if (incident.Decision != null)
            {
                if (incident.Decision.CriminalCase != null)
                {
                    _context.CriminalCases.Remove(incident.Decision.CriminalCase);
                }

                if (incident.Decision.TerritorialTransfer != null)
                {
                    _context.TerritorialTransfers.Remove(incident.Decision.TerritorialTransfer);
                }
                _context.Decisions.Remove(incident.Decision);
            }
            _context.Incidents.Remove(incident);

            _context.SaveChanges();
        }

    }
}
