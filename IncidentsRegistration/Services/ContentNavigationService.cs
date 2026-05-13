using IncidentsRegistration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace IncidentsRegistration.Services
{
    public class ContentNavigationService
        : IContentNavigationService
    {
        private Frame? _frame;
        private readonly IServiceProvider _services;

        public ContentNavigationService(
            IServiceProvider services)
        {
            _services = services;
        }

        public void SetFrame(Frame frame)
        {
            _frame = frame;
        }

        public void Navigate<T>(object? parameter = null) where T : Page
        {
            if (_frame == null)
                return;

            var page = _services.GetRequiredService<T>();

            if (parameter != null)
            {
                var vm = page.DataContext;
                if (vm != null)
                {
                    var init = vm.GetType().GetMethod("Initialize");
                    init?.Invoke(vm, new[] { parameter });
                }
            }

            _frame.Navigate(page);
        }

        public void GoBack()
        {
            if (_frame?.CanGoBack == true)
            {
                _frame.GoBack();
            }
        }
    }
}