using System;
using DevExpress.Mvvm;

namespace TimeCalculator
{
    public class RunTimeCalculatorItem : BindableBase
    {
        public TimeSpan StartTime
        {
            get => GetProperty(() => StartTime);
            set => SetProperty(() => StartTime, value);
        }

        public TimeSpan EndTime
        {
            get => GetProperty(() => EndTime);
            set => SetProperty(() => EndTime, value);
        }

        public RunTimeCalculatorItemType ItemType { get; set; }

        public TimeSpan? CalculateDiffernce()
        {
            if (EndTime < StartTime) return null;

            return EndTime - StartTime;
        }
    }
}