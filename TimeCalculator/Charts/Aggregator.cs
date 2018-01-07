using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using TimeCalculator.Charts.Builder;
using TimeCalculator.Charts.DTO;
using TimeCalculator.Data;

namespace TimeCalculator.Charts
{
    public static class Aggregator
    {
        private class RuntimeAvarangeCalculator
        {
            private int _count;
            private double _maximum;
            private double _avarange;

            public double Time => TimeSpan.FromMinutes(_avarange).TotalMilliseconds;

            public void Push(double elemnt)
            {
                _maximum += elemnt;
                _count++;

                _avarange = _maximum / _count;
            }
        }

        public static void Show<TType>()
        {
            ChartDatabase database;

            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (typeof(TType) == typeof(AvarageSetupTimePerQuater)) database = CreateAvarageSetupTimePerQuater();
            else database = null;

            if(database == null) return;

            var app = Application.Current;
            app.Dispatcher.Invoke(() =>
            {
                var window = new ChartsWindow(database) {Owner = app.MainWindow};
                window.ShowDialog();
            });
        }

        private static AvarageSetupTimePerQuaterBuilder CreateAvarageSetupTimePerQuater()
        {
            Dictionary<int, RuntimeAvarangeCalculator[]> items = new Dictionary<int, RuntimeAvarangeCalculator[]>();

            using (var db = DataBaseFactory.CreateDatabase())
            {
                foreach (var entity in db.JobEntities.AsNoTracking().Where(e => e.SetupTime != null).Select(e => new { e.StartTime, e.SetupTime }))
                {
                    int year = entity.StartTime.Year;
                    int quater = GetQuater(entity.StartTime.Month);
                    if (!items.ContainsKey(year))
                        items[year] = new[] { new RuntimeAvarangeCalculator(), new RuntimeAvarangeCalculator(), new RuntimeAvarangeCalculator(), new RuntimeAvarangeCalculator() };

                    items[year][quater - 1].Push((double)entity.SetupTime);
                }
            }

            return new AvarageSetupTimePerQuaterBuilder(
               items.Select(i => new AvarageSetupTimePerQuater(new DateTime(i.Key, 1, 1), i.Value[0].Time, i.Value[1].Time, i.Value[2].Time, i.Value[3].Time)).ToList());
        }

        private static int GetQuater(int month)
        {
            if (month <= 3)
                return 1;
            if (month <= 6)
                return 2;

            return month <= 9 ? 3 : 4;
        }
    }
}