using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using WPF.Core;
using WPF.Models;
using WPF.Models.Tabs;
using WPF.Services;
using WPF.ViewModels.Core;
using WPF.ViewModels.Trucks.Interfaces;

namespace WPF.ViewModels.Trucks
{
    public class TruckListViewModel : Tab, ITruckListViewModel
    {
        private IEnumerable<Truck> _truckList;
        private readonly IDataService _dataService;
        private Truck _selectedTruck;
        private TruckViewModel _truckViewModel;

        public IEnumerable<Truck> TruckList
        {
            get => _truckList?.Where(c => c.Make.ToLower().Contains("")) ?? SetProperty(ref _truckList, null);
            set => SetProperty(ref _truckList, value);
        }

        public Truck SelectedTruck
        {
            set
            {

                SetProperty(ref _selectedTruck, value);
                if (value == null && _selectedTruck == null) return;

                _truckViewModel.Truck = _selectedTruck.Clone() as Truck;

                SelectedTruck = null;
            }
            get => _selectedTruck;
        }



        public ICommand RefreshTruckListCommand { get; }
        public ICommand AddTruckCommand { get; }

        public TruckListViewModel(IDataService dataService)
        {
            _dataService = dataService;
            RefreshTruckListCommand = new RelayCommand(async delegate { await Refresh(); });
            AddTruckCommand = new RelayCommand(p => Add());
            _truckViewModel = new TruckViewModel(_dataService);
            _truckViewModel.TruckEvent += _truckViewModel_TruckEvent;
            SignalRNetCore();
        }

        private async Task _truckViewModel_TruckEvent(Truck truck)
        {
            await Task.Run(() =>
            {
                var row = TruckList.FirstOrDefault(c => c.TruckId == truck.TruckId);
                if (row != null)
                {
                    row.Make = truck.Make;
                    row.Model = truck.Model;
                    row.VIN = truck.VIN;
                    row.Color = truck.Color;
                    row.Year = truck.Year;

                }
                else
                {
                    var list = TruckList.ToList();
                    list.Add(truck);
                    TruckList = list.OrderBy(c => c.Year);
                }
                NotifyPropertyChanged("TruckList");
            });
        }

        private void Add()
        {
            SelectedTruck = new Truck();
        }

        private async Task Refresh()
        {
            TruckList = await _dataService.TruckService.GetAllTrucks("", "");

        }

        private void SignalRNetCore()
        {
            try
            {

                var truck = new HubConnectionBuilder()
                    .WithUrl("http://192.168.2.200:5000/hubs/truck")
                    .Build();
                //testGroupHub.On<object>("UpdateTestPlan", UpdateTestPlan);
                truck.On<object>("AddTruck", AddTruck);
                truck.On<object>("UpdateTruck", UpdateTruck);
                //testGroupHub.On<object>("DeleteTestPlan", DeleteTestPlan);
                //// Start the connection
                Task.Run(async () =>
                {
                    await truck.StartAsync();
                    Console.WriteLine("conecyado");
                });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void UpdateTruck(object obj)
        {
            var truck = JsonConvert.DeserializeObject<Truck>(obj.ToString());
            var row = TruckList.First(c => c.TruckId == truck.TruckId);
            var comparer = new ObjectsComparer.Comparer<Truck>();
            var k = comparer.Compare(truck, row, out _);
            if (k) return;
            row.Make = truck.Make;
            row.Model = truck.Model;
            row.VIN = truck.VIN;
            row.Color = truck.Color;
            row.Year = truck.Year;
            NotifyPropertyChanged("TruckList");
        }

        private void AddTruck(object obj)
        {
            var truck = JsonConvert.DeserializeObject<Truck>(obj.ToString());
            var row = TruckList.FirstOrDefault(c => c.TruckId == truck.TruckId);
            if (row == null)
            {
                var list = TruckList.ToList();
                list.Add(truck);
                TruckList = list.OrderBy(c => c.Year);
            }
            NotifyPropertyChanged("TruckList");
        }
    }
}