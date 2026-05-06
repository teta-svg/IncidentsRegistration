using CommunityToolkit.Mvvm.ComponentModel;
using IncidentsRegistration.Models;
using System.Data;

namespace IncidentsRegistration.ViewModels
{
    public partial class MainAppViewModel : ObservableObject
    {
        [ObservableProperty]
        private SystemUser _currentUser;

        [ObservableProperty]
        private string _role;

        public MainAppViewModel(SystemUser user, string role)
        {
            CurrentUser = user;
            Role = role?.Trim().ToLower();
        }

        public bool CanSeeAllIncidents => true;

        public bool CanSeeOperational => Role == "администратор" || Role == "руководитель группы";

        public bool IsLead => Role == "руководитель группы";
        public bool IsAdmin => Role == "администратор";
    }
}