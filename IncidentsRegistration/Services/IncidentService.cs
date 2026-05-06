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

        public void CreateIncident(Incident incident)
        {
            if (incident == null)
                throw new ArgumentNullException(nameof(incident));

            if (string.IsNullOrWhiteSpace(incident.TypeOfIncident))
                throw new ArgumentException("Укажите тип инцидента");

            incident.RegistrationDate = DateOnly.FromDateTime(DateTime.Now);
            incident.RegistrationTime = TimeOnly.FromDateTime(DateTime.Now);

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
                .Include(i => i.IdResponseTeamNavigation)
                .Include(i => i.IncidentLocations)
                    .ThenInclude(il => il.IdLocationNavigation)
                .Include(i => i.SubjectRoles);
        }

        public List<Incident> GetActiveIncidentsByTeam(int responseTeamId)
        {
            return BaseQuery()
                .Where(i => i.Decision == null)
                .Where(i => i.IdResponseTeam == responseTeamId)
                .ToList();
        }

        public Incident GetFullIncidentDetails(int incidentId)
        {
            return _context.Incidents
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
        }
    }
}
