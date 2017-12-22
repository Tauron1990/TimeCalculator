using System;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using JetBrains.Annotations;

namespace TimeCalculator
{
    public sealed class RunTimeCalculatorViewModel : ViewModelBase
    {
        public ObservableCollection<RunTimeCalculatorItem> CalculatorItems { get; }

        public RunTimeCalculatorViewModel()
        {
            CalculatorItems = new ObservableCollection<RunTimeCalculatorItem>();
            AddItemCommand = new DelegateCommand<RunTimeCalculatorItemType>(AddItem);
        }

        protected override void OnInitializeInDesignMode()
        {
            CalculatorItems.Add(new RunTimeCalculatorItem { EndTime = new TimeSpan(0,16,0,0), StartTime = new TimeSpan(0,7,0,0)});
            base.OnInitializeInDesignMode();
        }

        public RunTimeCalculatorItem Current
        {
            get => GetProperty(() => Current);
            set => SetProperty(() => Current, value);
        }

        public RuntimeCalculatorResult EffectiveTime => GetEffectiveTime();

        private RuntimeCalculatorResult GetEffectiveTime()
        {
            var result = new RuntimeCalculatorResult();

            TimeSpan? span = TimeSpan.Zero;

            foreach (var item in CalculatorItems)
            {
                switch (item.ItemType)
                {
                    case RunTimeCalculatorItemType.Iteration:
                        result.Iterations.Add(item);
                        break;
                    case RunTimeCalculatorItemType.Setup:
                        result.Setup = item;
                        break;
                }

                var temp = item.CalculateDiffernce();
                if (temp == null)
                {
                    span = null;
                    break;
                }

                span += temp.Value;
            }

            result.Runtime = span;

            return result;
        }

        public DelegateCommand<RunTimeCalculatorItemType> AddItemCommand { get; }

        private void AddItem(RunTimeCalculatorItemType parameter)
        {
            if(CalculatorItems.Count > 0 && parameter == RunTimeCalculatorItemType.Setup) return;

            var dateTime = DateTime.Now;

            CalculatorItems.Add(new RunTimeCalculatorItem { ItemType = parameter, StartTime = new TimeSpan(dateTime.Hour, dateTime.Minute, 0)});
        }

        [Command, UsedImplicitly]
        public void Remove() => CalculatorItems.Remove(Current);

        [UsedImplicitly]
        public bool CanRemove() => Current != null;
    }
}