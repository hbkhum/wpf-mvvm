using System;
using System.Threading.Tasks;
using WPF.Core;
using WPF.Models;
using WPF.Services;
using WPF.ViewModels.Cars.Interfaces;
using WPF.ViewModels.Core;

namespace WPF.ViewModels.Cars
{
    public class CarViewModel: ValidatableBindableBase, ICarViewModel
    {



        public delegate Task CarHandler(Car car);
        public event CarHandler CarEvent;

        private Car _car;
        private Car _carClone;
        private readonly IDataService _dataService;

        public RelayCommand UpdateCommand { get; }
        public RelayCommand CancelCommand { get; }

        public bool CanSave => _car != null && (!_car.HasErrors && _car.IsDirty);

        public Car Car
        {
            set
            {
                SetProperty(ref _car, value);
                _carClone = _car.Clone() as Car;
            }
            get => _car;
        }



        public CarViewModel(IDataService dataService)
        {
            _dataService = dataService;
            UpdateCommand = new RelayCommand(async delegate { await Update(); }, n => true);
            CancelCommand = new RelayCommand(async delegate { await Cancel(); }, n =>true);

        }

        private async Task Cancel()
        {
            if (Car.CarId != Guid.Empty)
            {
                Car = _carClone;
            }
            else
            {

            }
            


            //if (!ValidationHelper.IsFormValid(Car, page.CurrentPage)) { return; }
            await Task.Run(() =>
            {

                //_carClone = null;
            });
        }

        private async Task Update()
        {

            
            var flag = false;
            if (_car.CarId != Guid.Empty)
            {
                //var controller = await _metroWindow.ShowProgressAsync("Employee", "Employee Updated Successfully");
                flag = await _dataService.CarService.UpdateCar(_car);
            }
            else
            {
                //var controller = await _metroWindow.ShowProgressAsync("Employee", "Employee Added Successfully");
                flag = await _dataService.CarService.CreateCar(_car);
            }

            if (flag)
            {
                await OnCarEvent(Car);
            }

        }
        protected virtual async Task OnCarEvent(Car car)
        {
            await Task.Run(() => CarEvent?.Invoke(car));
        }
    }
}
