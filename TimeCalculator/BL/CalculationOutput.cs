using System;

namespace TimeCalculator.BL
{
    public class CalculationOutput
    {
        public string FormatedResult { get; }
        public TimeSpan? NormalizedTime { get; }

        public CalculationOutput(string formatedResult, TimeSpan? normalizedTime)
        {
            FormatedResult = formatedResult;
            NormalizedTime = normalizedTime;
        }
    }
}