using System.Collections.Generic;
using WPF.Core;
using WPF.ViewModels.Cars;
using WPF.ViewModels.Cars.Interfaces;
using WPF.ViewModels.Core;
using WPF.ViewModels.Layout;
using WPF.ViewModels.Trucks;
using WPF.ViewModels.Trucks.Interfaces;

namespace WPF.ViewModels
{
    public interface IMainPageViewModel
    {
        MenuViewModel MenuViewModel { get; }
    }
    public class MainPageViewModel : BindableBase, IMainPageViewModel
    {

        public  MenuViewModel MenuViewModel { get; private set; }



        private bool _flyOuLeftIsOpen;
        public bool FlyOutLeftIsOpen
        {
            get => _flyOuLeftIsOpen;
            set
            {
                _flyOuLeftIsOpen = value;
                this.
                    NotifyPropertyChanged("FlyOutLeftIsOpen");
            }
        }


        private RelayCommand _menuCommand;
        public RelayCommand MenuCommand => _menuCommand ??
                                           (_menuCommand = new RelayCommand(OnMenu, MenuCanExecute));

        private bool MenuCanExecute(object obj)
        {
            return true;
        }

        private void OnMenu(object obj)
        {
            FlyOutLeftIsOpen = !FlyOutLeftIsOpen;
        }

        public MainPageViewModel(IMenuViewModel menuViewModel)
        {
            MenuViewModel = (MenuViewModel)menuViewModel;

        }

        //private void Page_Popped(object sender, NavigationEventArgs e)
        //{
        //    

        //    ((NavigationPage)sender).Popped -= Page_Popped;
        //}

    }

}