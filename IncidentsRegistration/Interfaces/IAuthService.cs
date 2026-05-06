using IncidentsRegistration.Models;

namespace IncidentsRegistration.Interfaces
{
    public interface IAuthService
    {
        SystemUser? Login(string login, string password);
        void Register(string login, string password);
        string GetRole(SystemUser user);
    }
}
