using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Core;
using WPF.Models.Tabs;
using WPF.Services;
using WPF.ViewModels.Cars;
using WPF.ViewModels.Cars.Interfaces;
using WPF.ViewModels.Core;
using WPF.ViewModels.Trucks;
using WPF.ViewModels.Trucks.Interfaces;

namespace WPF.ViewModels.Layout
{
    public class MenuViewModel : BindableBase, IMenuViewModel
    {
        private readonly IDataService _dataService;
        private readonly CarListViewModel _carListViewModel;
        private readonly TruckListViewModel _truckListViewModel;
        public ICommand CarCommand { get; }

        public ICommand TruckCommand { get; }


        public ICollection<ITab> Tabs { get; }

        public MenuViewModel(
            ICarListViewModel carListViewModel,
            ITruckListViewModel truckListViewModel
            )
        {
            _carListViewModel = (CarListViewModel)carListViewModel;
            _truckListViewModel = (TruckListViewModel)truckListViewModel;
            CarCommand = new RelayCommand(p => CarTab());
            TruckCommand = new RelayCommand(p => TruckTab());


            var tabs = new ObservableCollection<ITab>();
            tabs.CollectionChanged += Tabs_CollectionChanged;
            Tabs = tabs;
        }



        private void Tabs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ITab tab;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    tab = (ITab)e.NewItems[0];
                    tab.CloseRequested += OnTabCloseRequested;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    tab = (ITab)e.OldItems[0];
                    tab.CloseRequested -= OnTabCloseRequested;
                    break;
            }
        }
        private void OnTabCloseRequested(object sender, EventArgs e)
        {
            Tabs.Remove((ITab)sender);
        }

        private void CarTab()
        {
            if (!Tabs.Any(t => t is CarListViewModel)) Tabs.Add(_carListViewModel);
            _carListViewModel.RefreshCarListCommand.Execute(null);

        }
        private void TruckTab()
        {

            if (!Tabs.Any(t => t is TruckListViewModel)) Tabs.Add(_truckListViewModel);
            _truckListViewModel.RefreshTruckListCommand.Execute(null);
        }

    }
}
