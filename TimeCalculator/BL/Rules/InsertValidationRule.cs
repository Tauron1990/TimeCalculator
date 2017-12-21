using System;
using TimeCalculator.Properties;

namespace TimeCalculator.BL.Rules
{
    public sealed class InsertValidationRule : ValidationRuleBase<ValidationOutput, ValidationInput>
    {
        public override ValidationOutput Action(ValidationInput input)
        {
            var settings = Settings.Default;

            if (!AssertList(new[]
            {
                new AssertHelp(input.Amount != null, "Auflage wurde nicht gesetzt."),
                new AssertHelp(input.Iteration != null, "Durchläufe wurde nicht gesetzt."),
                new AssertHelp(input.Amount > 0, "Auflage muss größer als 1 sein."),
                new AssertHelp(input.Iteration > 0, "Durchlauf muss midestens 1 haben."),
                new AssertHelp(input.Format.Success, "Papeier Format Angabe Falsch."),
                new AssertHelp(input.Format.Lenght > 29 && input.Format.Width > 19, "Format mindestens A4"),
                new AssertHelp(input.Speed != null, "Geschwindigkeit wurde nicht gesetzt."),
                new AssertHelp(input.Speed > 0, "Geschwindigkeit muss midestens 0,1 haben."),
                new AssertHelp(input.Speed <= 0.7, "Geschwindigkeit darf maximal 0,7 haben."),
                new AssertHelp(input.RunTime > TimeSpan.FromSeconds(1), "Die Laufzeit muss über 1 Sekunde Liegen.")
            }, out var formattedValue)) return new ValidationOutput(formattedValue, null);

            // ReSharper disable PossibleInvalidOperationException
            double iterationTime = input.Iteration.Value / (double)settings.IterationTime;

            TimeSpan realRunTime = input.RunTime - TimeSpan.FromMinutes(iterationTime);

            double runtime = realRunTime.TotalMinutes / input.Iteration.Value / input.Amount.Value * 1000d;

            TimeSpan? normalizedTime = TimeSpan.FromMinutes(runtime);

            formattedValue = $"Normalisierte Zeit: {normalizedTime.Value.Hours}:{normalizedTime.Value.Minutes} (per Tausend)";

            // ReSharper restore PossibleInvalidOperationException

            return new ValidationOutput(formattedValue, normalizedTime);
        }
    }
}