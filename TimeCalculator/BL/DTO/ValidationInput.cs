using System;

namespace TimeCalculator.BL
{
    public sealed class ValidationInput
    {
        public long? Amount { get; }
        public long? Iteration { get; }
        public TimeSpan RunTime { get; }
        public PaperFormat Format { get; }
        public double? Speed { get; }

        public ValidationInput(long? amount, long? iteration, TimeSpan runTime, PaperFormat format, double? speed)
        {
            Amount = amount;
            Iteration = iteration;
            RunTime = runTime;
            Format = format;
            Speed = speed;
        }
    }
}