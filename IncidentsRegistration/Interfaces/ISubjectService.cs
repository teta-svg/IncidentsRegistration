using IncidentsRegistration.Models;

namespace IncidentsRegistration.Interfaces
{
    public interface ISubjectService
    {
        void AddSubjectAndLinkToIncident(Subject subject, SubjectRole role);
        List<SubjectRole> GetParticipantsByIncident(int incidentId);
        void RemoveRole(int id);
        void UpdateSubjectAndRole(Subject updatedSubject, int incidentId, string roleName);
    }

}
