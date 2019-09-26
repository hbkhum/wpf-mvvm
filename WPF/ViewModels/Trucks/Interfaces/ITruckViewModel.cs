using WPF.Core;
using WPF.Models;

namespace WPF.ViewModels.Trucks.Interfaces
{
    public interface ITruckViewModel
    {
        Truck Truck { get; }
        RelayCommand UpdateCommand { get; }
        RelayCommand CancelCommand { get; }
    }
}