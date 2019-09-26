using System;

namespace WPF.Models
{
    public class Truck : Vehicle
    {
        private Guid _truckId;

        public Guid TruckId
        {
            get => _truckId;
            set => SetProperty(ref _truckId, value);
        }
    }
}