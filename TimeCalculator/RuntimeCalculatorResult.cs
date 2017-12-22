using System;
using System.Collections.Generic;

namespace TimeCalculator
{
    public sealed class RuntimeCalculatorResult
    {
        public List<RunTimeCalculatorItem> Iterations { get; }

        public RunTimeCalculatorItem Setup { get; set; }

        public TimeSpan? Runtime { get; set; }

        public RuntimeCalculatorResult()
        {
            Iterations = new List<RunTimeCalculatorItem>();
        }
    }
}