using System;
using TimeCalculator.Properties;

namespace TimeCalculator.BL.Rules
{
    public sealed class CalculationRule : ISimpleRule<CalculationOutput, CalculationInput>
    {
        private sealed class AssertHelp
        {
            public string Message { get;  }

            public bool Ok { get;  }

            public AssertHelp(bool ok, string message)
            {
                Ok = ok;
                Message = message;
            }
        }

        public CalculationOutput Action(CalculationInput input)
        {
            var settings = Settings.Default;

            TimeSpan? normalizedTime = null;

            if (AssertList(new[]
            {
                new AssertHelp(input.Amount != null, "Auflage wurde nicht gesetzt."),
                new AssertHelp(input.Iteration != null, "Durchläufe wurde nicht gesetzt."), 
                new AssertHelp(input.Amount > 0, "Auflage muss größer als 1 sein."),
                new AssertHelp(input.Iteration > 0, "Durchlauf mus midestens 1 haben."),
                new AssertHelp(input.RunTime > TimeSpan.FromSeconds(1), "Die Laufzeit muss über 1 Sekunde Liegen") 

            }, out var formattedValue))
            {
                // ReSharper disable PossibleInvalidOperationException
                double iterationTime = input.Iteration.Value / (double)settings.IterationTime;

                TimeSpan realRunTime = input.RunTime - TimeSpan.FromMinutes(iterationTime);

                double runtime = realRunTime.TotalMinutes / input.Amount.Value * 1000d;

                normalizedTime  = TimeSpan.FromMinutes(runtime);

                formattedValue = "Normalisierte Zeit: " + normalizedTime.Value.Hours + ":" + normalizedTime.Value.Minutes;

                // ReSharper restore PossibleInvalidOperationException
            }

            return new CalculationOutput(formattedValue, normalizedTime);
        }

        private bool Assert(bool predicate, string message, out string value)
        {
            if (predicate)
            {
                value = null;
                return true;
            }
            value = message;
            return false;
        }

        private bool AssertList(AssertHelp[] asserts, out string value)
        {
            value = null;
            foreach (var assert in asserts)
                if (!Assert(assert.Ok, assert.Message, out value)) return false;
            return true;
        }
    }
}