using IncidentsRegistration.Data;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using Microsoft.EntityFrameworkCore;

public class SubjectService : ISubjectService
{
    private readonly IncidentsDbContext _context;
    public SubjectService(IncidentsDbContext context) => _context = context;

    public void AddSubject(Subject subject)
    {
        _context.Subjects.Add(subject);
        _context.SaveChanges();
    }

    public List<SubjectRole> GetParticipantsByIncident(int incidentId)
    {
        return _context.SubjectRoles
            .AsNoTracking()
            .Include(sr => sr.IdSubjectNavigation)
            .Where(sr => sr.IdIncident == incidentId)
            .ToList();
    }

    public void RemoveRolesForPersonInIncident(int id)
    {
        var selectedRole = _context.SubjectRoles.Find(id);

        if (selectedRole != null)
        {
            var allPersonRoles = _context.SubjectRoles
                .Where(r => r.IdIncident == selectedRole.IdIncident
                         && r.IdSubject == selectedRole.IdSubject)
                .ToList();

            if (allPersonRoles.Any())
            {
                _context.SubjectRoles.RemoveRange(allPersonRoles);
                _context.SaveChanges();
            }
        }
    }

    public void UpdateSubject(Subject updatedSubject)
    {
        var existing = _context.Subjects
            .FirstOrDefault(s => s.IdSubject == updatedSubject.IdSubject);

        if (existing == null)
            throw new Exception("Subject not found");

        _context.Entry(existing).CurrentValues.SetValues(updatedSubject);

        _context.SaveChanges();
    }

    public void AddSubjectRole(int subjectId, int incidentId, string roleName)
    {
        var role = new SubjectRole
        {
            IdSubject = subjectId,
            IdIncident = incidentId,
            RoleName = roleName,
            DateOfInvolvement = DateOnly.FromDateTime(DateTime.Today)
        };

        _context.SubjectRoles.Add(role);
        _context.SaveChanges();
    }

    public SubjectRole? GetLastRole(int subjectId, int incidentId)
    {
        return _context.SubjectRoles
            .Where(r => r.IdSubject == subjectId && r.IdIncident == incidentId)
            .OrderByDescending(r => r.DateOfInvolvement)
            .ThenByDescending(r => r.IdSubjectRole)
            .FirstOrDefault();
    }

    public Subject? FindExistingSubjectByInn(string? inn)
    {
        if (string.IsNullOrWhiteSpace(inn))
            return null;

        return _context.Subjects
            .FirstOrDefault(s => s.Inn == inn);
    }


}
