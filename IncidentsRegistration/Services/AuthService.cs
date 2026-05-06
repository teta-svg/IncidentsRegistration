using IncidentsRegistration.Data;
using IncidentsRegistration.Interfaces;
using IncidentsRegistration.Models;
using Microsoft.EntityFrameworkCore;

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
            throw new ArgumentException("Пароль не подходит под требования безопасности");

        if (_context.SystemUsers.Any(u => u.LoginName == login))
            throw new ArgumentException("Пользователь с таким логином уже зарегистрирован");

        var user = new SystemUser
        {
            LoginName = login,
            UserPassword = password,
            UserRole = "диспетчер"
        };

        _context.SystemUsers.Add(user);
        _context.SaveChanges();
    }

    public string GetRole(SystemUser user)
    {
        return user.UserRole ?? "диспетчер";
    }

    private bool IsValidPassword(string password)
    {
        return !string.IsNullOrEmpty(password)
            && password.Length >= 6
            && password.Any(char.IsUpper)
            && password.Any(char.IsDigit)
            && password.Any(c => "!@#$%^".Contains(c));
    }
}
