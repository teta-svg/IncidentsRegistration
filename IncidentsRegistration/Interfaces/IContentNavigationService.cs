using System.Windows.Controls;

namespace IncidentsRegistration.Interfaces
{
    public interface IContentNavigationService
    {
        void SetFrame(Frame frame);

        void Navigate<T>(object? parameter = null)
            where T : Page;

        void GoBack();
    }
}