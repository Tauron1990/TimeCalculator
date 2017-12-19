using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

namespace TimeCalculator
{
    public sealed class MainWindowViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, string> _errorValues = new Dictionary<string, string>();

        public IEnumerable GetErrors(string propertyName) => !_errorValues.TryGetValue(propertyName, out var list) ? null : list;

        public bool HasErrors { get { return _errorValues.All(l => l.Value == null); } }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void AddError(string name, string value)
        {
            if (!_errorValues.ContainsKey(name))
                _errorValues[name] = null;

            if(_errorValues[name] == value) return;

            _errorValues[name] = value;

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(name));

            RaisePropertyChanged(nameof(HasErrors));
        }

        public MainWindowViewModel()
        {
            Reset();
        }

        [Command]
        public void Reset()
        {
            Amount = null;
            Iterations = null;
            StartDateTime = DateTime.Now;
            RunTime = null;
        }

        public long? Amount
        {
            get => GetProperty(() => Amount);
            set => SetProperty(() => Amount, value, () => ValidateLong(nameof(Amount), value));
        }

        public long? Iterations
        {
            get => GetProperty(() => Iterations);
            set => SetProperty(() => Iterations, value, () => ValidateLong(nameof(Iterations), value));
        }

        private void ValidateLong(string name, long? value) => AddError(name, value > 0 ? null : "Wert muss mindestens 1 sein.");

        public bool Problems
        {
            get => GetProperty(() => Problems);
            set
            {
                SetProperty(() => Problems, value, () =>
                {
                    if (value) BigProblems = false;
                });
            }
        }

        public bool BigProblems
        {
            get => GetProperty(() => BigProblems);
            set => SetProperty(() => BigProblems, value, () =>
            {
                if (value) Problems = false;
            });
        }

        public DateTime StartDateTime
        {
            get => GetProperty(() => StartDateTime);
            set => SetProperty(() => StartDateTime, value);
        }

        public TimeSpan? RunTime
        {
            get => GetProperty(() => RunTime);
            set => SetProperty(() => RunTime, value, () =>
            {
                if (value > TimeSpan.FromSeconds(1))
                    AddError(nameof(RunTime), "Mindesten eine Sekunde Laufzeit.");
                else
                    AddError(nameof(RunTime), null);
            });
        }
    }
}