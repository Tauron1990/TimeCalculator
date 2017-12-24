using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using JetBrains.Annotations;
using TimeCalculator.Data;

namespace TimeCalculator
{
    public sealed class RunTimeCalculatorViewModel : ViewModelBase
    {
        private const string RunTimeCalculatorProperty = nameof(RunTimeCalculatorProperty);

        public class ListExposingObservableCollection : ObservableCollection<RunTimeCalculatorItem>
        {
            public IList<RunTimeCalculatorItem> ToSerialize => Items;
        }

        public ListExposingObservableCollection CalculatorItems { get; }

        public RunTimeCalculatorViewModel()
        {
            CalculatorItems = new ListExposingObservableCollection();
            AddItemCommand = new DelegateCommand<RunTimeCalculatorItemType>(AddItem);
        }

        protected override void OnInitializeInDesignMode()
        {
            var curr = DateTime.Now;

            CalculatorItems.Add(new RunTimeCalculatorItem(RunTimeCalculatorItemType.Running, curr) { EndTime = curr + TimeSpan.FromHours(2)});
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

            CalculatorItems.Add(new RunTimeCalculatorItem(parameter, dateTime));

            SessionManager.PropertyChange();
        }

        [UsedImplicitly]
        public void AddItemSpecial()
        {
            if (CalculatorItems.Count == 0)
            {
                AddItem(RunTimeCalculatorItemType.Setup);
                return;
            }

            var dateTime = DateTime.Now;
            var last = CalculatorItems.Last();
            last.EndTime = dateTime;

            switch (last.ItemType)
            {
                case RunTimeCalculatorItemType.Iteration:
                case RunTimeCalculatorItemType.Setup:
                    AddItem(RunTimeCalculatorItemType.Running);
                    break;
                case RunTimeCalculatorItemType.Running:
                    AddItem(RunTimeCalculatorItemType.Iteration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SessionManager.PropertyChange();
        }

        [Command, UsedImplicitly]
        public void Remove() => CalculatorItems.Remove(Current);

        [UsedImplicitly]
        public bool CanRemove() => Current != null;

        public void Initialize(bool loadMode)
        {      
            if(loadMode)
                foreach (var item in SessionManager.Get<IList<RunTimeCalculatorItem>>(RunTimeCalculatorProperty))
                    CalculatorItems.Add(item);

            SessionManager.Set(RunTimeCalculatorProperty, CalculatorItems.ToSerialize);
        }
    }
}