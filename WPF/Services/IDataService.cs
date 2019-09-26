using System;
using System.Net.Http;
using WPF.Services.Interface;

namespace WPF.Services
{
    public interface IDataService
    {
        CarService CarService { get; set; }
        TruckService TruckService { get; set; }
    }

    public class DataService : IDataService
    {
        public DataService(ICarService carService, ITruckService truckService)
        {
            CarService = (CarService)carService;
            TruckService = (TruckService)truckService;

            foreach (var c in GetType().GetProperties())
            {
                var obj = c.GetValue(this, null);
                var type = obj.GetType();
                var info = type.GetProperty("Client");
                info?.SetValue(obj, new HttpClient { BaseAddress = new Uri("http://192.168.2.200:5000/api/") });
            }
        }

        public CarService CarService { get; set; }
        public TruckService TruckService { get; set; }
    }
}