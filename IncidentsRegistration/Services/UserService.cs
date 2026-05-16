using IncidentsRegistration.Data;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Services
{
    public class UserService : IUserService
    {
        private readonly IncidentsDbContext _context;

        public UserService(IncidentsDbContext context)
        {
            _context = context;
        }

        public List<SystemUser> GetAllUsers()
        {
            return _context.SystemUsers
                .Include(u => u.SystemUserResponseTeams)
                .ThenInclude(sur => sur.IdResponseTeamNavigation)
                .AsNoTracking()
                .ToList();
        }

        public List<ResponseTeam> GetAllTeams()
        {
            return _context.ResponseTeams.AsNoTracking().ToList();
        }

        public void AddUser(SystemUser user, int? teamId)
        {
            _context.SystemUsers.Add(user);
            _context.SaveChanges();

            if (user.UserRole == "руководитель группы" && teamId.HasValue)
            {
                _context.SystemUserResponseTeams.Add(new SystemUserResponseTeam
                {
                    IdUser = user.IdUser,
                    IdResponseTeam = teamId.Value
                });
                _context.SaveChanges();
            }
        }

        public void UpdateUser(SystemUser user, int? teamId)
        {
            var existing = _context.SystemUsers
                .Include(u => u.SystemUserResponseTeams)
                .FirstOrDefault(u => u.IdUser == user.IdUser);

            if (existing == null) return;

            existing.LoginName = user.LoginName;
            existing.UserPassword = user.UserPassword;
            existing.UserRole = user.UserRole;

            var currentLink = existing.SystemUserResponseTeams.FirstOrDefault();
            if (user.UserRole == "руководитель группы" && teamId.HasValue)
            {
                if (currentLink == null)
                    _context.SystemUserResponseTeams.Add(new SystemUserResponseTeam { IdUser = user.IdUser, IdResponseTeam = teamId.Value });
                else
                    currentLink.IdResponseTeam = teamId.Value;
            }
            else if (currentLink != null)
            {
                _context.SystemUserResponseTeams.Remove(currentLink);
            }

            _context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var user = _context.SystemUsers
                .Include(u => u.SystemUserResponseTeams)
                .FirstOrDefault(u => u.IdUser == userId);

            if (user == null)
                return;

            _context.SystemUserResponseTeams
                .RemoveRange(user.SystemUserResponseTeams);

            _context.SystemUsers.Remove(user);

            _context.SaveChanges();
        }
        public SystemUser? CurrentUser { get; set; }
    }
}
