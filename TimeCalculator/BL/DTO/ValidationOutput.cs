using System;

namespace TimeCalculator.BL
{
    public class ValidationOutput
    {
        public string FormatedResult { get; }
        public TimeSpan? NormalizedTime { get; }

        public ValidationOutput(string formatedResult, TimeSpan? normalizedTime)
        {
            FormatedResult = formatedResult;
            NormalizedTime = normalizedTime;
        }
    }
}