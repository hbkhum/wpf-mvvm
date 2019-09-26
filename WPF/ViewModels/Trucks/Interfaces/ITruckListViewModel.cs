using System.Collections.Generic;
using System.Windows.Input;
using WPF.Models;

namespace WPF.ViewModels.Trucks.Interfaces
{
    public interface ITruckListViewModel
    {
        IEnumerable<Truck> TruckList { get; }
        Truck SelectedTruck { get; }
        ICommand RefreshTruckListCommand { get; }
        ICommand AddTruckCommand { get; }
    }
}