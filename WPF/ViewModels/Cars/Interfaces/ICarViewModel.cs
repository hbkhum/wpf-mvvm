using WPF.Core;
using WPF.Models;

namespace WPF.ViewModels.Cars.Interfaces
{
    public interface ICarViewModel
    {
        Car Car { get; }
        RelayCommand UpdateCommand { get; }
        RelayCommand CancelCommand { get; }
    }
}