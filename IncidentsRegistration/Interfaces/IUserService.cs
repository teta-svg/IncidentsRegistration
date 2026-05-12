using IncidentsRegistration.Models;

namespace IncidentsRegistration.Interfaces
{
    public interface IUserService
    {
        List<SystemUser> GetAllUsers();
        List<ResponseTeam> GetAllTeams();
        SystemUser? CurrentUser { get; set; }
        void AddUser(SystemUser user, int? teamId);
        void UpdateUser(SystemUser user, int? teamId);
        void DeleteUser(int userId);
    }
}
