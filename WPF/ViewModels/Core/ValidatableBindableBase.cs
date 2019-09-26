using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WPF.ViewModels.Core
{
    public abstract class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs>
            ErrorsChanged = delegate { };

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        }

        public bool HasErrors => _errors.Count > 0;

        protected override T SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {

            base.SetProperty<T>(ref member, val, propertyName);
            ValidateProperty(propertyName, val);
            return member;
        }

        private void ValidateProperty<T>(string propertyName, T value)
        {
            var results = new List<ValidationResult>();

            var context = new ValidationContext(this) { MemberName = propertyName };
            Validator.TryValidateProperty(value, context, results);

            if (results.Any())
            {
                _errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                _errors.Remove(propertyName);
            }

            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
