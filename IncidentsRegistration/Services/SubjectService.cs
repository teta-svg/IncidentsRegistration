using IncidentsRegistration.Data;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IncidentsDbContext _context;
        public SubjectService(IncidentsDbContext context) => _context = context;

        public void AddSubjectAndLinkToIncident(Subject subject, SubjectRole role)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Subjects.Add(subject);
                _context.SaveChanges();

                role.IdSubject = subject.IdSubject;
                _context.SubjectRoles.Add(role);

                _context.SaveChanges();
                transaction.Commit();
            }
            catch { transaction.Rollback(); throw; }
        }
        public List<SubjectRole> GetParticipantsByIncident(int incidentId)
        {
            return _context.SubjectRoles
                .Include(sr => sr.IdSubjectNavigation)
                .Where(sr => sr.IdIncident == incidentId)
                .ToList();
        }

        public void RemoveRole(int id)
        {
            var role = _context.SubjectRoles.Find(id);
            if (role != null)
            {
                _context.SubjectRoles.Remove(role);
                _context.SaveChanges();
            }
        }

    }

}
