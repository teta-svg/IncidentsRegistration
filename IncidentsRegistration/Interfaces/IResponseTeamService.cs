using IncidentsRegistration.Models;

namespace IncidentsRegistration.Interfaces
{
    public interface IResponseTeamService
    {
        List<ResponseTeam> GetFreeTeams();
        void AssignTeam(int incidentId, int teamId);
        List<ResponseTeam> GetAllTeams();
    }
}
