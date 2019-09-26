using System;
using System.Windows.Input;
using WPF.Core;
using WPF.ViewModels.Core;

namespace WPF.Models.Tabs
{
    public abstract class Tab : BindableBase, ITab
    {
        protected Tab()
        {
            CloseCommand = new RelayCommand(p => CloseRequested?.Invoke(this, EventArgs.Empty));
        }
        public string Name { get; set; }
        public ICommand CloseCommand { get; }
        public event EventHandler CloseRequested;
        //public FontAwesomeIcon Icon { get; set; }
    }
}