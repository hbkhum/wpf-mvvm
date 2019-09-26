using Microsoft.Extensions.DependencyInjection;
using WPF.Services;
using WPF.Services.Interface;
using WPF.ViewModels.Cars;
using WPF.ViewModels.Cars.Interfaces;
using WPF.ViewModels.Layout;
using WPF.ViewModels.Trucks;
using WPF.ViewModels.Trucks.Interfaces;

namespace WPF.ViewModels
{
    public class ViewModelLocator
    {
        private static MainPageViewModel _mainPageViewModel;
        public static MainPageViewModel MainPageViewModelStatic
        {
            get
            {

                if (_mainPageViewModel != null) return _mainPageViewModel;
                var serviceProvider = ServiceCollection();

                _mainPageViewModel = (MainPageViewModel)serviceProvider.GetService<IMainPageViewModel>(); //new ();
                return _mainPageViewModel;
            }
        }

        private static ServiceProvider ServiceCollection()
        {
            var serviceProvider = new ServiceCollection()

                .AddSingleton<IMenuViewModel, MenuViewModel>()
                .AddScoped(typeof(IClientService<>), typeof(ClientService<>))

                .AddSingleton<ICarListViewModel, CarListViewModel>()
                .AddSingleton<ICarViewModel, CarViewModel>()
                .AddSingleton<ITruckListViewModel, TruckListViewModel>()
                .AddSingleton<ITruckViewModel, TruckViewModel>()
                .AddSingleton<IMainPageViewModel, MainPageViewModel>()

                .AddSingleton<IDataService, DataService>()
                .AddSingleton<ICarService, CarService>()
                .AddSingleton<ITruckService, TruckService>()



                .BuildServiceProvider();
            //var l = serviceProvider.GetServices(typeof(IClientService<>));
            return serviceProvider;
        }
    }


}