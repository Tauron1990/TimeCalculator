using System;
using System.Collections.Generic;
using TimeCalculator.BL.Rules;

namespace TimeCalculator.BL
{
    public static class BusinessRules
    {
        private static readonly Dictionary<string, object> Rules = new Dictionary<string, object>();

        private static ISimpleRule<TOutput, TInput> GetRule<TOutput, TInput>(string name,
            Func<ISimpleRule<TOutput, TInput>> constructor)
        {
            if (Rules.TryGetValue(name, out var value)) return (ISimpleRule<TOutput, TInput>) value;

            var rule = constructor();
            Rules[name] = rule;

            return rule;
        }

        public static ISimpleRule<ValidationOutput, ValidationInput> InsertValidationRule => GetRule(nameof(InsertValidationRule), () => new InsertValidationRule());
        public static ISimpleRule<SaveOutput, SaveInput> SaveRule => GetRule(nameof(SaveRule), () => new SaveRule());
        public static ISimpleRule<CalculateTimeOutput, CalculateTimeInput> CalculateTimeRule => GetRule(nameof(CalculateTimeRule), () => new CalculateTimeRule());
        public static ISimpleRule<CalculateValidateOutput, CalculateTimeInput> CalculateValidationRule => GetRule(nameof(CalculateValidationRule), () => new CalculationValidationRule());
    }
}