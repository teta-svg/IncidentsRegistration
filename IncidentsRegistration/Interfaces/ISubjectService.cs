using IncidentsRegistration.Models;

namespace IncidentsRegistration.Interfaces
{
    public interface ISubjectService
    {
        void UpdateSubject(Subject subject);

        void AddSubjectRole(int subjectId, int incidentId, string roleName);

        public void AddSubject(Subject subject);

        List<SubjectRole> GetParticipantsByIncident(int incidentId);

        void RemoveRolesForPersonInIncident(int id);

        SubjectRole? GetLastRole(int subjectId, int incidentId);

        Subject? FindExistingSubjectByInn(string? inn);
    }

}
