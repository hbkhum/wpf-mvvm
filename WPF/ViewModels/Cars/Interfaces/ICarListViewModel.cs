using System.Collections.Generic;
using System.Windows.Input;
using WPF.Models;

namespace WPF.ViewModels.Cars.Interfaces
{
    public interface ICarListViewModel
    {
        IEnumerable<Car> CarList { get; }
        Car SelectedCar { get; }
        ICommand RefreshCarListCommand { get; }
        ICommand AddCarCommand { get; }
    }
}