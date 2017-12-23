using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;
using MathNet.Numerics;
using Microsoft.EntityFrameworkCore;
using TimeCalculator.Data;

namespace TimeCalculator.BL.Rules
{
    public sealed class CalculateTimeRule : ISimpleRule<CalculateTimeOutput, CalculateTimeInput>
    {
        public CalculateTimeOutput Action([NotNull] CalculateTimeInput input)
        {
            var valOutput = BusinessRules.CalculateValidation.Action(input);
            if (!valOutput.Valid)
                return new CalculateTimeOutput(null, null, null, valOutput.Message, PrecisionMode.InValid);

            Dictionary<PrecisionMode, List<JobEntity>> dic = AggregateEntitys(input);

            var setupTime = TimeSpan.FromMinutes(Properties.Settings.Default.SetupTime);

            // ReSharper disable PossibleInvalidOperationException

            var iterationTime = TimeSpan.FromMinutes(Properties.Settings.Default.IterationTime * (int) input.Iterations);
            var effectiveAmount = CalculateEffectiveAmount((int) input.Amount, (int) input.Iterations);


            if (dic[PrecisionMode.Perfect].Count > 0)
                return new CalculateTimeOutput(setupTime, iterationTime,
                    FindPerfectRuntime(dic[PrecisionMode.Perfect], effectiveAmount), "OK", PrecisionMode.Perfect);

            if (dic[PrecisionMode.AmountMismatchPerfect].Count > 0)
                return new CalculateTimeOutput(setupTime, iterationTime,
                    FindPerfectRuntime(dic[PrecisionMode.AmountMismatchPerfect], effectiveAmount), "OK",
                    PrecisionMode.AmountMismatchPerfect);

            if (dic[PrecisionMode.NearCorner].Count > 0)
                return new CalculateTimeOutput(setupTime, iterationTime,
                    FindNearCornerRuntime(dic[PrecisionMode.NearCorner], effectiveAmount, input.Speed.Value),
                    "OK", PrecisionMode.NearCorner);

            if(dic[PrecisionMode.AmountMismatchNearCorner].Count > 0)
                return new CalculateTimeOutput(setupTime, iterationTime,
                    FindNearCornerRuntime(dic[PrecisionMode.AmountMismatchNearCorner], effectiveAmount, input.Speed.Value),
                    "OK", PrecisionMode.AmountMismatchNearCorner);
            // ReSharper restore PossibleInvalidOperationException

            return new CalculateTimeOutput(null, null, null, "Keine Daten Verfügbar.", PrecisionMode.NoData);
        }

        private int CalculateEffectiveAmount(int amount, int iterations)
        {
            return amount * iterations;
        }

        private TimeSpan FindPerfectRuntime(List<JobEntity> entrys, int amount) => 
            TimeSpan.FromMinutes(entrys.Average(e => e.NormaizedTime.TotalMinutes) * (amount / 1000d));

        private ImmutableSortedDictionary<double, List<JobEntity>> Sort(IEnumerable<JobEntity> entry, double speed)
        {
            Dictionary<double, List<JobEntity>> sortDic = new Dictionary<double, List<JobEntity>>();          

            foreach (var clusterEntry in entry)
            {
                double clusterSpeed = clusterEntry.Speed;

                List<double> potential = new List<double>(sortDic.Keys) {speed};
                bool added = false;

                foreach (var key in potential)
                {
                    if (key.AlmostEqual(clusterSpeed, Properties.Settings.Default.PefectDifference))
                    {
                        sortDic[speed].Add(clusterEntry);
                        added = true;
                        break;
                    }
                }

                if(added) continue;

                if(!sortDic.ContainsKey(clusterSpeed))
                    sortDic[clusterSpeed] = new List<JobEntity>{clusterEntry};
                else
                    sortDic[clusterSpeed].Add(clusterEntry);
            }

            return sortDic.ToImmutableSortedDictionary();
        }

        private TimeSpan FindNearCornerRuntime(IEnumerable<JobEntity> entrys, int amount, double speed)
        {
            List<double> points = new List<double>();
            List<double> values = new List<double>();

            foreach (var speedEntry in Sort(entrys, speed))
            {
                points.Add(speedEntry.Key);
                values.Add(speedEntry.Value.Average(ce => ce.NormaizedTime.TotalMinutes));
            }

            return TimeSpan.FromMinutes(Interpolate.Common(points, values).Interpolate(speed) * (amount / 1000d));
        }

        private Dictionary<PrecisionMode, List<JobEntity>> AggregateEntitys(CalculateTimeInput input)
        {
            Dictionary<PrecisionMode, List<JobEntity>> cluster = new Dictionary<PrecisionMode, List<JobEntity>>
            {
                {PrecisionMode.Perfect, new List<JobEntity>()},
                {PrecisionMode.AmountMismatchPerfect, new List<JobEntity>()},
                {PrecisionMode.NearCorner, new List<JobEntity>()},
                {PrecisionMode.AmountMismatchNearCorner, new List<JobEntity>()},
            };

            if (input.PaperFormat.Lenght == null) return cluster;
            if (input.Speed == null) return cluster;
            if (input.Amount == null) return cluster;

            double speed = input.Speed.Value;

            var settings = Properties.Settings.Default;
            int lenght = input.PaperFormat.Lenght.Value;
            long datetimeTicks = DateTime.Now.Ticks;
            int currentQuatal = CalculateQuatal(DateTime.Now);
            long expire = settings.EntityExpire;
            double tolerance = settings.NearCornerDifference;
            long amount = input.Amount.Value;


            using (var database = new JobDatabase())
            {
                foreach (var entry in database.JobEntities.AsQueryable()
                    .Where(e => e.Length == lenght)
                    .Where(e => e.Speed.AlmostEqual(speed, tolerance))
                    .Where(e => e.StartTime.Ticks + expire < datetimeTicks)
                    .Where(e => CalculateQuatal(e.StartTime) == currentQuatal)
                    .AsNoTracking())
                {
                    if (entry.Speed.AlmostEqual(speed, settings.PefectDifference))
                    {
                        if (IsAmoutMismatch(entry.Amount, amount)) cluster[PrecisionMode.AmountMismatchPerfect].Add(entry);
                        else cluster[PrecisionMode.Perfect].Add(entry);
                    }
                    else if (entry.Speed.AlmostEqual(speed, settings.NearCornerDifference))
                    {
                        if (IsAmoutMismatch(entry.Amount, amount)) cluster[PrecisionMode.AmountMismatchNearCorner].Add(entry);
                        else cluster[PrecisionMode.NearCorner].Add(entry);
                    }
                }
            }

            return cluster;
        }

        private bool IsAmoutMismatch(long set, long ist)
        {
            int tolerance = Properties.Settings.Default.AmoutMismatch;

            return ((double) set).AlmostEqualNumbersBetween(ist, tolerance);
        }
        private int CalculateQuatal(DateTime dateTime)
        {
            if (dateTime.Month < 4) return 1;
            if (dateTime.Month < 7) return 2;
            if (dateTime.Month < 10) return 3;
            return 4;
        }
    }
}