using System;
using System.Runtime.Serialization;
using DevExpress.Mvvm;

namespace TimeCalculator
{
    [Serializable]
    public class RunTimeCalculatorItem : BindableBase, ISerializable
    {
        public DateTime StartTime
        {
            get => GetProperty(() => StartTime);
            set => SetProperty(() => StartTime, value);
        }

        public DateTime EndTime
        {
            get => GetProperty(() => EndTime);
            set => SetProperty(() => EndTime, value);
        }

        public RunTimeCalculatorItemType ItemType { get; }

        public RunTimeCalculatorItem(RunTimeCalculatorItemType itemType, DateTime startTime)
        {
            StartTime = startTime;
            ItemType = itemType;

            switch (ItemType)
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

        protected RunTimeCalculatorItem(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            StartTime = (DateTime) info.GetValue(nameof(StartTime), typeof(DateTime));
            EndTime = (DateTime) info.GetValue(nameof(EndTime), typeof(DateTime));
            ItemType = (RunTimeCalculatorItemType) info.GetValue(nameof(ItemType), typeof(RunTimeCalculatorItemType));
        }

        public TimeSpan? CalculateDiffernce()
        {
            if (EndTime < StartTime) return null;

            return EndTime - StartTime;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(StartTime), StartTime);
            info.AddValue(nameof(EndTime), EndTime);
            info.AddValue(nameof(ItemType), ItemType);
        }
    }
}