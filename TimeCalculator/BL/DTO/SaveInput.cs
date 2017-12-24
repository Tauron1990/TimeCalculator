using System;
using System.Collections.Generic;

namespace TimeCalculator.BL
{
    public sealed class SaveInput
    {
        public long? Amount { get; }
        public long? Iteratins { get; }
        public bool Problem { get; }
        public bool BigProblem { get; }
        public PaperFormat PaperFormat { get; }
        public double? Speed { get; }
        public DateTime StartTime { get; }
        public TimeSpan RunTime { get; }

        public SaveInput(long? amount, long? iteratins, bool problem, bool bigProblem, PaperFormat paperFormat, double? speed, DateTime startTime, TimeSpan runTime)
        {
            Amount = amount;
            Iteratins = iteratins;
            Problem = problem;
            BigProblem = bigProblem;
            PaperFormat = paperFormat;
            Speed = speed;
            StartTime = startTime;
            RunTime = runTime;
        }
    }
}