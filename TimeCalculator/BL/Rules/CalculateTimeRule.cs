using System.Collections.Generic;
using TimeCalculator.Data;

namespace TimeCalculator.BL.Rules
{
    public sealed class CalculateTimeRule : ISimpleRule<CalculateTimeOutput, CalculateTimeInput>
    {


        public CalculateTimeOutput Action(CalculateTimeInput input)
        {
            var valOutput = BusinessRules.CalculateValidationRule.Action(input);
            if(!valOutput.Valid) return new CalculateTimeOutput(null, null, null, valOutput.Message, PrecisionMode.NoData);


        }

        private IEnumerable<JobEntity> AggregateEntitys(CalculateTimeInput input)
        {

        }
    }
}