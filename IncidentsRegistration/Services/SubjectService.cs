using IncidentsRegistration.Data;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using Microsoft.EntityFrameworkCore;

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
            .AsNoTracking()
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

    public void UpdateSubjectAndRole(Subject updatedSubject, int incidentId, string roleName)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var tracked = _context.Subjects.Local.FirstOrDefault(s => s.IdSubject == updatedSubject.IdSubject);
            if (tracked != null) _context.Entry(tracked).State = EntityState.Detached;
            updatedSubject.SubjectRoles.Clear();

            _context.Subjects.Update(updatedSubject);
            _context.SaveChanges();

            var newRoleEntry = new SubjectRole
            {
                IdIncident = incidentId,
                IdSubject = updatedSubject.IdSubject,
                RoleName = roleName,
                DateOfInvolvement = DateOnly.FromDateTime(DateTime.Today)
            };

            _context.SubjectRoles.Add(newRoleEntry);
            _context.SaveChanges();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
