using IncidentsRegistration.Models;

namespace IncidentsRegistration.Interfaces
{
    public interface IResponseTeamService
    {
        List<ResponseTeam> GetFreeTeams();
        void AssignTeam(int incidentId, int teamId);
        List<ResponseTeam> GetAllTeams();
        void AddTeam(ResponseTeam team);
        void UpdateTeam(ResponseTeam team);
        void DeleteTeam(int teamId);
    }
}
