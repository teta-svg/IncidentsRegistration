using System.Windows.Controls;

namespace IncidentsRegistration.Interfaces
{
    public interface INavigationService
    {
        void SetFrame(Frame frame);
        void Navigate<T>(object? parameter = null) where T : Page;
        void GoBack();
    }
}
