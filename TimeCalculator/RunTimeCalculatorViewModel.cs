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
            AddItemCommand = new DelegateCommand(AddItem);
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

        public TimeSpan? EffectiveTime => GetEffectiveTime();

        private TimeSpan? GetEffectiveTime()
        {
            TimeSpan span = TimeSpan.Zero;

            foreach (var item in CalculatorItems)
            {
                var temp = item.CalculateDiffernce();
                if (temp == null)
                    return null;

                span += temp.Value;
            }

            return span;
        }

        public DelegateCommand AddItemCommand { get; }

        public void AddItem()
        {
            CalculatorItems.Add(new RunTimeCalculatorItem());
        }

        [Command, UsedImplicitly]
        public void Remove() => CalculatorItems.Remove(Current);

        [UsedImplicitly]
        public bool CanRemove() => Current != null;
    }
}