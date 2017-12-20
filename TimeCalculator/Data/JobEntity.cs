using System;
using System.ComponentModel.DataAnnotations;

namespace TimeCalculator.Data
{
    public sealed class JobEntity
    {
        [Key]
        public int Id { get; set; }

        public bool Problem { get; set; }

        public bool BigProblem { get; set; }

        public long Iterations { get; set; }

        public long Amount { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public double Speed { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan NormaizedTime { get; set; }

        public TimeSpan EffectiveTime { get; set; }
    }
}