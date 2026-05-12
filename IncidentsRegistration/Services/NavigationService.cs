using IncidentsRegistration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

public class NavigationService : INavigationService
{
    private Frame? _frame;
    private readonly IServiceProvider _services;

    public NavigationService(IServiceProvider services)
    {
        _services = services;
    }

    public void SetFrame(Frame frame)
    {
        _frame = frame;
    }

    public void Navigate<T>(object? parameter = null) where T : Page
    {
        if (_frame == null) return;

        var page = _services.GetRequiredService<T>();

        if (parameter != null)
        {
            var init = page.DataContext?.GetType().GetMethod("Initialize");
            init?.Invoke(page.DataContext, new[] { parameter });
        }

        _frame.Navigate(page);
    }

    public void GoBack()
    {
        _frame?.GoBack();
    }
}