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
    public class TruckService : ClientService<Truck>, ITruckService
    {



        public async Task<bool> CreateTruck(Truck entity)
        {
            var result = false;
            var response = await CreateEntityAsync(entity);
            if (response.StatusCode != HttpStatusCode.OK) return await Task.FromResult(false);
            entity.IsDirty = false;
            var data = await response.Content.ReadAsStringAsync();
            entity.TruckId = new Guid(data.Replace("\"", ""));
            if (entity.TruckId != Guid.NewGuid()) result = true;
            return await Task.FromResult(result);
            //NotifyPropertyChanged("Employee");
            //var controller = await _metroWindow.ShowProgressAsync("Employee", "Employee Added Successfully");
            //await Task.Delay(500);
            //controller.SetIndeterminate();
            //await controller.CloseAsync();
        }

        public Task<bool> DeleteTruck(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Truck>> GetAllTrucks(string whereValue, string orderBy)
        {
            var response = await GetAll(whereValue, orderBy);
            var data = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(data)["result"];
            var obj = JsonConvert.DeserializeObject<List<Truck>>(json.ToString());
            var trucks = obj.OrderBy(c => c.Year).ToList();
            trucks.ForEach(c => c.IsDirty = false);
            return trucks;
        }

        public async Task<bool> UpdateTruck(Truck entity)
        {
            var response = await UpdateEntityAsync(entity, "Truck/" + entity.TruckId);
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

        public async Task<Truck> GetTruckByIdAsync(Guid? id)
        {
            //throw new NotImplementedException();
            var response = await GetById(id);
            var data = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<Truck>(data);
            var truck = obj;
            //trucks.ForEach(c => c.IsDirty = false);
            return truck;
            //GetById
        }
    }
}