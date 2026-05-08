using IncidentsRegistration.Data;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Services
{
    public class ResponseTeamService: IResponseTeamService
    {
        private readonly IncidentsDbContext _context;

        public ResponseTeamService(IncidentsDbContext context)
        {
            _context = context;
        }

        public List<ResponseTeam> GetFreeTeams()
        {
            return _context.ResponseTeams
                .Include(rt => rt.Incidents)
                .Where(rt => rt.Incidents.All(i =>
                _context.Decisions.Any(d => d.IdIncident == i.IdIncident)))
                .ToList();
        }

        public void AssignTeam(int incidentId, int teamId)
        {
            var incident = _context.Incidents.Find(incidentId);

            if (incident == null)
                throw new ArgumentException("Инцидент не найден");

            var team = _context.ResponseTeams
                .Include(t => t.Incidents)
                .FirstOrDefault(t => t.IdResponseTeam == teamId);

            if (team == null)
                throw new ArgumentNullException(nameof(team));

            var isBusy = team.Incidents.Any(i => i.Decision == null);

            if (isBusy)
                throw new InvalidOperationException("Команда уже занята на другом инциденте");

            incident.IdResponseTeam = teamId;

            _context.SaveChanges();

        }

        public List<ResponseTeam> GetAllTeams()
        {
            return _context.ResponseTeams
                .AsNoTracking()
                .ToList();
        }

        public void AddTeam(ResponseTeam team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));

            _context.ResponseTeams.Add(team);
            _context.SaveChanges();
        }

        public void UpdateTeam(ResponseTeam team)
        {
            var existingTeam = _context.ResponseTeams.Find(team.IdResponseTeam);

            if (existingTeam == null)
                throw new ArgumentException("Команда не найдена");

            existingTeam.NameTeam = team.NameTeam;
            existingTeam.DirectorLastName = team.DirectorLastName;
            existingTeam.DirectorFirstName = team.DirectorFirstName;
            existingTeam.DirectorPatronymic = team.DirectorPatronymic;
            existingTeam.NumberOfPeople = team.NumberOfPeople;

            _context.SaveChanges();
        }

        public void DeleteTeam(int teamId)
        {
            var team = _context.ResponseTeams.Find(teamId);

            if (team == null)
                throw new ArgumentException("Команда не найдена");
            bool hasIncidents = _context.Incidents.Any(i => i.IdResponseTeam == teamId);
            if (hasIncidents)
                throw new InvalidOperationException("Нельзя удалить команду, у которой есть история инцидентов");

            _context.ResponseTeams.Remove(team);
            _context.SaveChanges();
        }

    }
}
