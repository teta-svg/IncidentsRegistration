using IncidentsRegistration.Data;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Services
{
    public class AuthService : IAuthService
    {
        private readonly IncidentsDbContext _context;

        public AuthService(IncidentsDbContext context)
        {
            _context = context;
        }

        public SystemUser? Login(string login, string password)
        {
           return _context.SystemUsers
                .Include(u => u.SystemUserResponseTeams)
                .FirstOrDefault(u => u.LoginName == login && u.UserPassword == password);
        }

        public void Register(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Введите логин");

            if (!IsValidPassword(password))
                throw new ArgumentException("Пароль не подходит");

            var exists = _context.SystemUsers
                .Any(u => u.LoginName == login);

            if (exists)
                throw new ArgumentException("Пользователь уже существует");

            var user = new SystemUser
            {
                LoginName = login,
                UserPassword = password
            };

            _context.SystemUsers.Add(user);
            _context.SaveChanges();
        }

        public string GetRole(SystemUser user)
        {
            if (user.LoginName == "Admin_User" && user.UserPassword == "AdminPass123@")
                return "Admin";

            if (user.SystemUserResponseTeams.Any())
                return "Team";

            return "Dispatcher";
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 6
                && password.Any(char.IsUpper)
                && password.Any(char.IsDigit)
                && password.Any(c => "!@#$%^".Contains(c));
        }
    }
}
