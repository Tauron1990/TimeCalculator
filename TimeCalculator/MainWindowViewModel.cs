using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using TimeCalculator.BL;

namespace TimeCalculator
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            PropertyChanged += OnPropertyChanged;
            Reset();
            SetReult();
        }
        
        [Command]
        public void Reset()
        {
            Amount = null;
            Iterations = null;
            StartDateTime = DateTime.Now;
            RunTime = TimeSpan.Zero;
            Problems = false;
            BigProblems = false;
        }

        public long? Amount
        {
            get => GetProperty(() => Amount);
            set => SetProperty(() => Amount, value);
        }

        public long? Iterations
        {
            get => GetProperty(() => Iterations);
            set => SetProperty(() => Iterations, value);
        }
        
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

        public string FormatValue
        {
            get => GetProperty(() => FormatValue);
            set => SetProperty(() => FormatValue, value);
        }

        public DateTime StartDateTime
        {
            get => GetProperty(() => StartDateTime);
            set => SetProperty(() => StartDateTime, value);
        }

        public TimeSpan RunTime
        {
            get => GetProperty(() => RunTime);
            set => SetProperty(() => RunTime, value);
        }

        public string Result
        {
            get => GetProperty(() => Result);
            set => SetProperty(() => Result, value);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case nameof(RunTime):
                case nameof(Amount):
                case nameof(Iterations):
                    SetReult();
                    break;
            }
        }

        private void SetReult()
        {
            var cresult = BuissinesRules.CalculationRule.Action(new CalculationInput(Amount, Iterations, RunTime));

            Result = cresult.FormatedResult;

            _insertOk = cresult.NormalizedTime != null;
        }

        private bool _insertOk;

        public string Status
        {
            get => GetProperty(() => Status);
            set => SetProperty(() => Status, value);
        }

        public bool StatusOk { get; set; }

        [Command]
        public void Save()
        {

        }
    }
}