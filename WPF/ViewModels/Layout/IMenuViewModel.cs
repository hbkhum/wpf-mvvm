using System.Collections.Generic;
using System.Windows.Input;
using WPF.Models.Tabs;

namespace WPF.ViewModels.Layout
{
    public interface IMenuViewModel
    {
        ICommand CarCommand { get; }
        ICommand TruckCommand { get; }
        ICollection<ITab> Tabs { get; }
    }
}