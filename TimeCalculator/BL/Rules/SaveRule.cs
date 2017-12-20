using TimeCalculator.Data;

namespace TimeCalculator.BL.Rules
{
    public sealed class SaveRule : ISimpleRule<SaveOutput, SaveInput>
    {
        public SaveOutput Action(SaveInput input)
        {
            var normalizedResult = BusinessRules.ValidationRule.Action(new ValidationInput(input.Amount, input.Iteratins, input.RunTime, input.PaperFormat, input.Speed));

            if(normalizedResult.NormalizedTime == null) return new SaveOutput(false, normalizedResult.FormatedResult);

            using (var database = new JobDatabase())
            {
                database.JobEntities.Add(new JobEntity
                {
                    // ReSharper disable PossibleInvalidOperationException
                    Amount = input.Amount.Value,
                    BigProblem = input.BigProblem,
                    Problem = input.Problem,
                    EffectiveTime = input.RunTime,
                    Iterations = input.Iteratins.Value,
                    Length = input.PaperFormat.Lenght.Value,
                    NormaizedTime = normalizedResult.NormalizedTime.Value,
                    Speed = input.Speed.Value,
                    StartTime = input.StartTime,
                    Width = input.PaperFormat.Width.Value
                    // ReSharper restore PossibleInvalidOperationException
                });

                database.SaveChanges();
            }

            return new SaveOutput(true, string.Empty);
        }
    }
}