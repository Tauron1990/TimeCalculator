﻿using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Native;
using JetBrains.Annotations;
using TimeCalculator.BL;
using TimeCalculator.Charts;
using TimeCalculator.Charts.DTO;
using TimeCalculator.Data;
using TimeCalculator.Properties;

namespace TimeCalculator
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        private const string RuntimeCalculatorProperty = nameof(RuntimeCalculatorProperty);
        private const string InProgressProperty = nameof(InProgressProperty);
        
        #region Common

        private bool _loadTimeCalculatorFromSession;

        public MainWindowViewModel()
        {
            PropertyChanged += OnPropertyChanged;
            _speedNotes = new SpeedNotes();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case nameof(CalcAmount):
                case nameof(CalcIterations):
                case nameof(CalcPaperFormat):
                case nameof(CalcDrops):
                case nameof(CalcSpeed):
                    SetResultCalculation();
                    break;
                case nameof(PaperFormat):
                case nameof(RunTime):
                case nameof(Amount):
                case nameof(Iterations):
                case nameof(Speed):
                    SetReultOfInsert();
                    break;
            }
        }

        public int SelectedTabIndex
        {
            get => GetProperty(() => SelectedTabIndex);
            set => SetProperty(() => SelectedTabIndex, value);
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            InitializeSettings();

            _loadTimeCalculatorFromSession = SessionManager.Get<bool>(RuntimeCalculatorProperty);

            if (SessionManager.Get<bool>(InProgressProperty)) return;

            Reset();
            SetResultCalculation();
        }

        public void WindowLoaded()
        {
            if(!SessionManager.Get<bool>(InProgressProperty))
                SelectedTabIndex = 1;

            if(_loadTimeCalculatorFromSession)
                CalculateTimeImpl(false, true);
        }

        protected override PropertyManager CreatePropertyManager()
        {
            var temp = SessionManager.CreateManager(nameof(MainWindowViewModel));

            string format = temp.GetProperty<string>(nameof(PaperFormat));
            string calcFormat = temp.GetProperty<string>(nameof(CalcPaperFormat));

            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                PaperFormat = format;
                CalcPaperFormat = calcFormat;
            }), DispatcherPriority.ApplicationIdle);

            return temp;
        }

        #endregion

        #region Operations

        public bool IsOperationRunning
        {
            get => GetProperty(() => IsOperationRunning);
            set => SetProperty(() => IsOperationRunning, value);
        }

        public string OperationTitle
        {
            get => GetProperty(() => OperationTitle);
            set => SetProperty(() => OperationTitle, value);
        }

        private void RunOperation(string title, Action operation)
        {
            OperationTitle = title;
            IsOperationRunning = true;
            Task.Run(() =>
            {
                operation();
                IsOperationRunning = false;
            });
        }

        #endregion

        #region Insert

        private int? _setupTime;
        private int? _iterationTime;

        [Command, UsedImplicitly]
        public void Reset()
        {
            ResetCalc();
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
            SetReultOfInsert();

            SelectedTabIndex = 1;

            SessionManager.Set(RuntimeCalculatorProperty, false);
            SessionManager.Set(InProgressProperty, false);
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

        private void SetReultOfInsert()
        {
            if (_saveOperationCompled) Application.Current.Dispatcher.BeginInvoke(new Action(Reset), DispatcherPriority.Background);
            
            var cresult = BusinessRules.InsertValidation.Action(new ValidationInput(Amount, Iterations, RunTime, new PaperFormat(PaperFormat), Speed));

            Result = cresult.FormatedResult;

            _insertOk = cresult.NormalizedTime != null;

            Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
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
            RunOperation("Speichere Job",() =>
            {
                try
                {
                    var result = BusinessRules.Save.Action(new SaveInput(Amount, Iterations, Problems, BigProblems, new PaperFormat(PaperFormat), Speed, 
                                                                         StartDateTime, RunTime, _setupTime, _iterationTime));

                    if (result.Succsess)
                    {
                        Status = "Speichern Erfolgreich";
                        StatusOk = true;
                        _iterationTime = null;
                        _setupTime = null;
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

        [Command, UsedImplicitly]
        public void CalculateTime()
        {
            CalculateTimeImpl(false);
        }

        private void CalculateTimeImpl(bool addSetup, bool loadFormSessionManager = false)
        {
            SessionManager.Set(RuntimeCalculatorProperty, true);

            try
            {
                var view = new RunTimeCalculator(addSetup, loadFormSessionManager) {Owner = Application.Current.MainWindow, WindowStartupLocation = WindowStartupLocation.CenterOwner};

                if (view.ShowDialog() != true) return;


                RunOperation("Laufzeit Berechnung", () =>
                {
                    var eff = view.EffectiveTime;

                    BusinessRules.AddSetupItems.Action(new AddSetupInput(eff.Iterations.Concat(new[] {eff.Setup})));
                    BusinessRules.RecalculateSetup.Action(null);

                    if (eff.Runtime == null) return;

                    _setupTime = eff.Setup.CalculateDiffernce()?.Minutes;
                    _iterationTime = eff.Iterations.Sum(i => i.CalculateDiffernce()?.Minutes);

                    RunTime = eff.Runtime.Value;
                    Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
                });
            }
            finally
            {
                SessionManager.Set(RuntimeCalculatorProperty, false);
            }
        }

        #endregion

        #region Calculation

        private SpeedNotes _speedNotes;
        private bool _canCalculate;
        private TimeSpan? _fullRunTime;
        private bool _calculationSuccessFull;
        private DateTime _calcTime;

        public string CalculatetRunTime
        {
            get => GetProperty(() => CalculatetRunTime);
            set => SetProperty(() => CalculatetRunTime, value);
        }

        public string CalculatetSetupTime
        {
            get => GetProperty(() => CalculatetSetupTime);
            set => SetProperty(() => CalculatetSetupTime, value);
        }

        public string CalculatetFullTime
        {
            get => GetProperty(() => CalculatetFullTime);
            set => SetProperty(() => CalculatetFullTime, value);
        }

        public string CalculationStatus
        {
            get => GetProperty(() => CalculationStatus);
            set => SetProperty(() => CalculationStatus, value);
        }


        public long? CalcAmount
        {
            get => GetProperty(() => CalcAmount);
            set => SetProperty(() => CalcAmount, value);
        }

        public long? CalcIterations
        {
            get => GetProperty(() => CalcIterations);
            set => SetProperty(() => CalcIterations, value);
        }

        public string CalcPaperFormat
        {
            get => GetProperty(() => CalcPaperFormat);
            set => SetProperty(() => CalcPaperFormat, value);
        }

        public double? CalcSpeed
        {
            get => GetProperty(() => CalcSpeed);
            set => SetProperty(() => CalcSpeed, value);
        }

        public long? CalcDrops
        {
            get => GetProperty(() => CalcDrops);
            set => SetProperty(() => CalcDrops, value, CalcDropsChanged);
        }

        private void CalcDropsChanged()
        {
            if(CalcDrops == null) return;

            CalcSpeed = _speedNotes.CalculateSpeed((int) CalcDrops.Value);
        }

        private void SetResultCalculation()
        {
            var result = BusinessRules.CalculateValidation.Action(new CalculateTimeInput(CalcIterations, new PaperFormat(CalcPaperFormat), CalcAmount, CalcSpeed));

            CalculationStatus = !result.Valid ? result.Message : "Start Bereit";

            _canCalculate = result.Valid;
            _calculationSuccessFull = false;

            Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
        }

        [Command, UsedImplicitly]
        public void Calculate()
        {
            RunOperation("Zeit Wird Berechnet", () =>
            {
                SessionManager.Set(InProgressProperty, true);
                _calcTime = DateTime.Now;

                var output = BusinessRules.CalculateTime.Action(new CalculateTimeInput(CalcIterations, new PaperFormat(CalcPaperFormat), CalcAmount, CalcSpeed));
                if (output.PrecisionMode == PrecisionMode.InValid || output.PrecisionMode == PrecisionMode.NoData)
                {
                    CalculatetFullTime = "0";
                    CalculatetRunTime = "0";
                    CalculatetSetupTime = "0";
                    CalculationStatus = output.Error;
                    _calculationSuccessFull = output.PrecisionMode != PrecisionMode.InValid;
                    _fullRunTime = null;
                }
                else
                {
                    _fullRunTime = output.IterationTime + output.RunTime + output.SetupTime;
                    CalculationStatus = $"OK ({FormatPrecision(output.PrecisionMode)})";
                    CalculatetRunTime = FormatTime(output.RunTime);
                    CalculatetFullTime = FormatTime(_fullRunTime);
                    CalculatetSetupTime = $"{FormatTime(output.IterationTime + output.SetupTime)} (Einrichtezeit: {FormatTime(output.SetupTime)} - Durchlaufzeit: {FormatTime(output.IterationTime)})";
                    _calculationSuccessFull = true;
                }

                Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
            });
        }

        private string FormatTime(TimeSpan? value) => value?.ToString("hh:mm");

        private string FormatPrecision(PrecisionMode precisionMode)
        {
            switch (precisionMode)
            {
                case PrecisionMode.Perfect:
                    return "Hohe Zuverlässigkeit";
                case PrecisionMode.NearCorner:
                    return "Mittlere Zuverlässigkeit";
                case PrecisionMode.AmountMismatchPerfect:
                    return "Hochrechnumt mit Mittlerer Zuverlässigkeit";
                case PrecisionMode.AmountMismatchNearCorner:
                    return "Hochrechnung mit Nidriger Zuverlässigkeit";
                case PrecisionMode.NoData:
                    return "Keine Daten";
                default:
                    return string.Empty;
            }
        }

        [UsedImplicitly]
        public bool CanCalculate() => _canCalculate;

        private void ResetCalc()
        {
            CalcAmount = null;
            CalcDrops = null;
            CalcIterations = null;
            CalcSpeed = null;
            CalcPaperFormat = null;
            CalculatetFullTime = string.Empty;
            CalculatetRunTime = string.Empty;
            CalculatetSetupTime = string.Empty;
        }

        [Command, UsedImplicitly]
        public void Exchange()
        {
            PaperFormat = CalcPaperFormat;
            RunTime = _fullRunTime ?? TimeSpan.Zero;
            Speed = CalcSpeed;
            Amount = CalcAmount;
            Iterations = CalcIterations;
            StartDateTime = _calcTime;

            SelectedTabIndex = 0;
            CalculateTimeImpl(true);
        }

        [UsedImplicitly]
        public bool CanExchange() => _calculationSuccessFull;

        [Command, UsedImplicitly]
        public void SpeedNoteEditor()
        {
            if(new SpeedNodeEditorWindow { Owner = Application.Current.MainWindow, WindowStartupLocation = WindowStartupLocation.CenterOwner}.ShowDialog() == true)
                _speedNotes = new SpeedNotes();
        }

        #endregion

        #region Settings

        private readonly Settings _settings = Settings.Default;

        public int IterationTimeSetting
        {
            get => _settings.IterationTime;
            set { _settings.IterationTime = value; _settings.Save(); }
        }

        public int SetupTimeSetting
        {
            get => _settings.SetupTime;
            set { _settings.SetupTime = value; _settings.Save(); }
        }

        public double DifferenzPerfectSetting
        {
            get => _settings.PefectDifference;
            set { _settings.PefectDifference = value; _settings.Save(); }
        }

        public double DifferenzNearSetting
        {
            get => _settings.NearCornerDifference;
            set { _settings.NearCornerDifference = value; _settings.Save(); }
        }

        public int AmoutToleranceSetting
        {
            get => _settings.AmoutMismatch;
            set { _settings.AmoutMismatch = value; _settings.Save(); }
        }

        public TimeSpan TimeExpireSetting
        {
            get => TimeSpan.FromTicks(_settings.EntityExpire);
            set { _settings.EntityExpire = value.Ticks; _settings.Save(); }
        }

        public string UserName
        {
            get => _settings.CurrentUser;
            set
            {
                if(string.IsNullOrWhiteSpace(value)) return;
                _settings.CurrentUser = value; _settings.Save();
            }
        }

        private void InitializeSettings()
        {
            _settings.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(_settings.IterationTime):
                        RaisePropertyChanged(nameof(IterationTimeSetting));
                        break;
                    case nameof(_settings.SetupTime):
                        RaisePropertyChanged(nameof(SetupTimeSetting));
                        break;
                }
            };
        }

        #endregion

        #region Charts

        [Command, UsedImplicitly]
        public void OpenCharts() => RunOperation("Aggregiere Daten", Aggregator.Show<AvarageSetupTimePerQuater>);

        #endregion
    }
}