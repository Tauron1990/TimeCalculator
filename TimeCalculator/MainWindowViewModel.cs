using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using JetBrains.Annotations;
using TimeCalculator.BL;

namespace TimeCalculator
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            PropertyChanged += OnPropertyChanged;
            Reset();
        }

        #region Operations

        public bool IsOperationRunning
        {
            get => GetProperty(() => IsOperationRunning);
            set => SetProperty(() => IsOperationRunning, value);
        }

        private void RunOperation(Action operation)
        {
            IsOperationRunning = true;
            operation();
            IsOperationRunning = false;
        }

        #endregion

        #region Insert

        [Command, UsedImplicitly]
        public void Reset()
        {
            Speed = null;
            Amount = null;
            Iterations = null;
            StartDateTime = DateTime.Now;
            RunTime = TimeSpan.Zero;
            Problems = false;
            BigProblems = false;
            PaperFormat = null;
            Status = string.Empty;
            _saveOperationCompled = false;
            SetReult();
        }

        public double? Speed
        {
            get => GetProperty(() => Speed);
            set => SetProperty(() => Speed, value);
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

        public string PaperFormat
        {
            get => GetProperty(() => PaperFormat);
            set => SetProperty(() => PaperFormat, value);
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
                case nameof(PaperFormat):
                case nameof(RunTime):
                case nameof(Amount):
                case nameof(Iterations):
                case nameof(Speed):
                    SetReult();
                    break;
            }
        }

        private void SetReult()
        {
            if (_saveOperationCompled) Application.Current.Dispatcher.BeginInvoke(new Action(Reset), DispatcherPriority.Background);

            var cresult = BusinessRules.ValidationRule.Action(new ValidationInput(Amount, Iterations, RunTime, new PaperFormat(PaperFormat), Speed));

            Result = cresult.FormatedResult;

            _insertOk = cresult.NormalizedTime != null;

            CommandManager.InvalidateRequerySuggested();
        }

        private bool _insertOk;
        private bool _saveOperationCompled;

        public string Status
        {
            get => GetProperty(() => Status);
            set => SetProperty(() => Status, value);
        }

        public bool StatusOk
        {
            get => GetProperty(() => StatusOk);
            set => SetProperty(() => StatusOk, value);
        }

        [Command, UsedImplicitly]
        public void Save()
        {
            RunOperation(() =>
            {
                try
                {
                    var result = BusinessRules.SaveRule.Action(new SaveInput(Amount, Iterations, Problems, BigProblems, new PaperFormat(PaperFormat), Speed, StartDateTime, RunTime));

                    if (result.Succsess)
                    {
                        Status = "Speichern Erfolgreich";
                        StatusOk = true;
                    }
                    else
                    {
                        Status = result.Message;
                        StatusOk = false;
                    }
                }
                catch (Exception e)
                {
                    if (e is OutOfMemoryException || e is Win32Exception || e is StackOverflowException)
                        throw;

                    File.WriteAllText("Exception.log", e.ToString());
                    StatusOk = false;

                    Status = $"Fehler: {e.GetType()}-{e.Message}";
                }

                _saveOperationCompled = true;
            });
        }

        [UsedImplicitly]
        public bool CanSave()
        {
            return !_saveOperationCompled && _insertOk;
        }

        #endregion
    }
}