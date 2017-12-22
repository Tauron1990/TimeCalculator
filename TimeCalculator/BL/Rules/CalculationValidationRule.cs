namespace TimeCalculator.BL.Rules
{
    public sealed class CalculationValidationRule : ValidationRuleBase<CalculateValidateOutput, CalculateTimeInput>
    {
        public override CalculateValidateOutput Action(CalculateTimeInput input)
        {
            bool valid = AssertList(new[]
            {
                new AssertHelp(input.Speed > 0, "Geschwindigkeit muss größer als 0 sein."), 
                new AssertHelp(input.Speed <= 0.7, "Geschwindigkeit darf maximal 0,7 haben."),
                new AssertHelp(input.Amount > 0, "Auflage muss midestens 1 sein."),
                new AssertHelp(input.Iterations > 0, "Mindesten ein Durchlauf muss Gesetzt werden."),
                new AssertHelp(input.PaperFormat.Success, "Papierformat muss gesetzt werden"), 

            }, out var msg);

            return new CalculateValidateOutput(valid, msg);
        }
    }
}