using System;

namespace WPF.Models
{
    public class Car:Vehicle
    {
        private Guid _carId;

        public Guid CarId
        {
            get => _carId;
            set => SetProperty(ref _carId, value);
        }
    }
}