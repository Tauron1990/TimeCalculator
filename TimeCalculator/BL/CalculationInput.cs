using System;

namespace TimeCalculator.BL
{
    public sealed class CalculationInput
    {
        public long? Amount { get; }
        public long? Iteration { get; }
        public TimeSpan RunTime { get; }

        public CalculationInput(long? amount, long? iteration, TimeSpan runTime)
        {
            Amount = amount;
            Iteration = iteration;
            RunTime = runTime;
        }
    }
}