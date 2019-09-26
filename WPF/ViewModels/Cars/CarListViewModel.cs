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
using WPF.ViewModels.Cars.Interfaces;
using WPF.ViewModels.Core;

namespace WPF.ViewModels.Cars
{
    public class CarListViewModel: Tab, ICarListViewModel
    {
        private IEnumerable<Car> _carList;
        private readonly IDataService _dataService;
        private Car _selectedCar;
        private CarViewModel _carViewModel;
        public CarViewModel CarViewModel
        {
            get => _carViewModel ?? SetProperty(ref _carViewModel, null);
            set => SetProperty(ref _carViewModel, value);
        }
        public IEnumerable<Car> CarList
        {
            get => _carList?.Where(c => c.Make.ToLower().Contains("")) ?? SetProperty(ref _carList, null);
            set => SetProperty(ref _carList, value);
        }

        public Car SelectedCar
        {
            set
            {

                SetProperty(ref _selectedCar, value);
                if (value == null && _selectedCar == null) return;

                _carViewModel.Car = _selectedCar.Clone() as Car;
                CarViewModel.Car = _selectedCar;

                SelectedCar = null;
            }
            get => _selectedCar;
        }



        public ICommand RefreshCarListCommand { get; }
        public ICommand AddCarCommand { get; }

        public CarListViewModel(IDataService dataService,ICarViewModel carViewModel)
        {
            Name = "Cars";
            _dataService = dataService;
            CarViewModel = (CarViewModel) carViewModel;
            RefreshCarListCommand = new RelayCommand(async delegate { await Refresh(); });
            AddCarCommand = new RelayCommand(p => Add());
            _carViewModel = new CarViewModel(_dataService);
            _carViewModel.CarEvent += _carViewModel_CarEvent;
            SignalRNetCore();
            //_carViewModel = (CarViewModel) carViewModel;
        }

        private async Task _carViewModel_CarEvent(Car car)
        {
            await Task.Run(() =>
            {
                var row = CarList.FirstOrDefault(c => c.CarId == car.CarId);
                if (row != null)
                {
                    row.Make = car.Make;
                    row.Model = car.Model;
                    row.VIN = car.VIN;
                    row.Color = car.Color;
                    row.Year = car.Year;

                }
                else
                {
                    var list = CarList.ToList();
                    list.Add(car);
                    CarList = list.OrderBy(c => c.Year);
                }
                NotifyPropertyChanged("CarList");
            });
        }

        private void Add()
        {
            SelectedCar = null;
            NotifyPropertyChanged("SelectedCar");
            SelectedCar = new Car();
            //if (_carViewModel != null) _carViewModel.Car = _selectedCar;
        }

        private async Task Refresh()
        {
            CarList = await _dataService.CarService.GetAllCars("", "");

        }

        private void SignalRNetCore()
        {
            try
            {

                var car = new HubConnectionBuilder()
                    .WithUrl("http://192.168.2.200:5000/hubs/car")
                    .Build();
                //testGroupHub.On<object>("UpdateTestPlan", UpdateTestPlan);
                car.On<object>("AddCar", AddCar);
                car.On<object>("UpdateCar", UpdateCar);
                //testGroupHub.On<object>("DeleteTestPlan", DeleteTestPlan);
                //// Start the connection
                Task.Run(async () =>
                {
                    await car.StartAsync();
                    Console.WriteLine("conecyado");
                });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void UpdateCar(object obj)
        {
            var car = JsonConvert.DeserializeObject<Car>(obj.ToString());
            var row = CarList.First(c => c.CarId == car.CarId);
            var comparer = new ObjectsComparer.Comparer<Car>();
            var k = comparer.Compare(car, row, out _);
            if (k) return;
            row.Make = car.Make;
            row.Model = car.Model;
            row.VIN = car.VIN;
            row.Color = car.Color;
            row.Year = car.Year;
            NotifyPropertyChanged("CarList");
        }

        private void AddCar(object obj)
        {
            var car = JsonConvert.DeserializeObject<Car>(obj.ToString());
            var row = CarList.FirstOrDefault(c => c.CarId == car.CarId);
            if (row == null)
            {
                var list = CarList.ToList();
                list.Add(car);
                CarList = list.OrderBy(c => c.Year);
            }
            NotifyPropertyChanged("CarList");
        }
    }
}