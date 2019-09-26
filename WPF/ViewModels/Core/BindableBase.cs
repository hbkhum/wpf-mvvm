using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF.ViewModels.Core
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public bool IsDirty { get; set; }
        protected virtual T SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {

            var comparer = new ObjectsComparer.Comparer<T>();
            IsDirty = member is Guid
                ? IsDirty || ((!Equals(member, Guid.Empty)) && (!comparer.Compare(member, val, out _)))
                : IsDirty || (((member != null && !comparer.Compare(member, val, out _)) || (member == null || val == null)));
            if (Equals(member, val)) return member;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            return member;
        }

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }


}
