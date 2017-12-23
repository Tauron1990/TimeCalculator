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

        public RunTimeCalculatorItem(RunTimeCalculatorItemType itemType, TimeSpan startTime)
        {
            StartTime = startTime;
            itemType = ItemType;

            switch (itemType)
            {
                case RunTimeCalculatorItemType.Iteration:
                    EndTime = startTime + TimeSpan.FromMinutes(Properties.Settings.Default.IterationTime);
                    break;
                case RunTimeCalculatorItemType.Setup:
                    EndTime = startTime + TimeSpan.FromMinutes(Properties.Settings.Default.SetupTime);
                    break;
                case RunTimeCalculatorItemType.Running:
                    EndTime = startTime + TimeSpan.FromHours(1);
                    break;
            }
        }

        public TimeSpan? CalculateDiffernce()
        {
            if (EndTime < StartTime) return null;

            return EndTime - StartTime;
        }
    }
}