using System;
using System.ComponentModel.DataAnnotations;
using WPF.ViewModels.Core;

namespace WPF.Models
{
    /// <summary>
    /// Person Class
    /// </summary>
    public abstract class Vehicle : ValidatableBindableBase, ICloneable, IVehicle
    {
        private string _model;
        private string _make;
        private string _vin;
        private string _color;
        private int _year;

        [Required]
        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }
        [Required]
        public string Make
        {
            get => _make;
            set => SetProperty(ref _make, value);
        }
        [Required]
        public string VIN
        {
            get => _vin;
            set => SetProperty(ref _vin, value);
        }
        [Required]
        public string Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        [Required]
        public int Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}