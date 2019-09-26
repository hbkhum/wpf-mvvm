using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WPF.Models;
using WPF.Services.Interface;

namespace WPF.Services
{
    public class CarService : ClientService<Car>, ICarService
    {
        //public EmployeeService(HttpClient client) 
        //    :base(client)
        //{
        //    //_clientService = clientService;
        //    //_metroWindow = (Application.Current.MainWindow as MetroWindow);
        //    //if (_metroWindow != null) _metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Inverted;
        //}



        public async Task<bool> CreateCar(Car entity)
        {
            var result = false;
            var response = await CreateEntityAsync(entity);
            if (response.StatusCode != HttpStatusCode.OK) return await Task.FromResult(false);
            entity.IsDirty = false;
            var data = await response.Content.ReadAsStringAsync();
            entity.CarId = new Guid(data.Replace("\"", ""));
            if (entity.CarId != Guid.NewGuid()) result = true;
            return await Task.FromResult(result);
            //NotifyPropertyChanged("Employee");
            //var controller = await _metroWindow.ShowProgressAsync("Employee", "Employee Added Successfully");
            //await Task.Delay(500);
            //controller.SetIndeterminate();
            //await controller.CloseAsync();
        }

        public Task<bool> DeleteCar(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Car>> GetAllCars(string whereValue, string orderBy)
        {
            var response = await GetAll(whereValue, orderBy);
            var data = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(data)["result"];
            var obj = JsonConvert.DeserializeObject<List<Car>>(json.ToString());
            var cars = obj.OrderBy(c => c.Year).ToList();
            cars.ForEach(c => c.IsDirty = false);
            return cars;
        }

        public async Task<bool> UpdateCar(Car entity)
        {
            var response = await UpdateEntityAsync(entity, "Car/" + entity.CarId);
            if (response.StatusCode != HttpStatusCode.OK) return await Task.Run(() => false);
            entity.IsDirty = false;
            var data = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => bool.Parse(data));

            //NotifyPropertyChanged("Employee");
            //var controller = await _metroWindow.ShowProgressAsync("Employee", "Employee Update Successfully");
            //await Task.Delay(500);
            //controller.SetIndeterminate();
            //await controller.CloseAsync();
        }

        public async Task<Car> GetCarByIdAsync(Guid? id)
        {
            //throw new NotImplementedException();
            var response = await GetById(id);
            var data = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<Car>(data);
            var car = obj;
            //cars.ForEach(c => c.IsDirty = false);
            return car;
            //GetById
        }
    }
}