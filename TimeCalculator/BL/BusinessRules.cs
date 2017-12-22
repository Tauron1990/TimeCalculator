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

        public static ISimpleRule<ValidationOutput, ValidationInput> InsertValidation => GetRule(nameof(InsertValidation), () => new InsertValidationRule());
        public static ISimpleRule<SaveOutput, SaveInput> Save => GetRule(nameof(Save), () => new SaveRule());
        public static ISimpleRule<CalculateTimeOutput, CalculateTimeInput> CalculateTime => GetRule(nameof(CalculateTime), () => new CalculateTimeRule());
        public static ISimpleRule<CalculateValidateOutput, CalculateTimeInput> CalculateValidation => GetRule(nameof(CalculateValidation), () => new CalculationValidationRule());
        public static ISimpleRule<object, AddSetupInput> AddSetupItems => GetRule(nameof(AddSetupItems), () => new AddSetupItemRule());
        public static ISimpleRule<object, object> RecalculateSetup => GetRule(nameof(RecalculateSetup), () => new RecalculateSetupRule());
    }
}