using CommunityToolkit.Mvvm.ComponentModel;
using IncidentsRegistration.Models;

namespace IncidentsRegistration.ViewModels
{
    public partial class MainAppViewModel : ObservableObject
    {
        public SystemUser CurrentUser { get; }
        public string Role { get; }

        public bool IsAdmin => Role == "Admin";
        public bool IsTeam => Role == "Team";
        public bool IsDispatcher => Role == "Dispatcher";

        public bool CanSeeAllIncidents => IsAdmin || IsTeam || IsDispatcher;

        public bool CanSeeOperational => IsAdmin || IsTeam;

        public MainAppViewModel(SystemUser user, string role)
        {
            CurrentUser = user;
            Role = role;
        }
    }
}