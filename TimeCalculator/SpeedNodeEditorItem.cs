using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Mvvm;

namespace TimeCalculator
{
    public sealed class SpeedNodeEditorItem : BindableBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        public SpeedNodeEditorItem()
        {
            Drops = 0;
            Speed = 0;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (!_dictionary.ContainsKey(propertyName) || string.IsNullOrWhiteSpace(_dictionary[propertyName]))
                return null;

            return new ArrayList { _dictionary[propertyName] };
        }

        public bool HasErrors => _dictionary.Count >= 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void OnErrorsChanged(DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

        private void AddError(string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (_dictionary.ContainsKey(propertyName))
                {
                    _dictionary.Remove(propertyName);
                    OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                }
                return;
            }

            if(_dictionary[propertyName] == value) return;

            _dictionary[propertyName] = value;
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }

        public double Speed
        {
            get => GetProperty(() => Speed);
            set => SetProperty(() => Speed, value, SpeedChangedCallback);
        }

        private void SpeedChangedCallback()
        {
            if(Speed < 0.06)
                AddError(nameof(Speed), "Geschwindigkeit muss über 0,06 m/s");
            else if(Speed > 0.7)
                AddError(nameof(Speed), "Geschwindigkeit");
            else 
                AddError(nameof(Speed), null);
        }

        public double Drops
        {
            get => GetProperty(() => Drops);
            set => SetProperty(() => Drops, value);
        }
    }
}