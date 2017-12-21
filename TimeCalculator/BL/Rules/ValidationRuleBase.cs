namespace TimeCalculator.BL.Rules
{
    public abstract class ValidationRuleBase<TOutput, TInput> : ISimpleRule<TOutput, TInput>
    {
        protected sealed class AssertHelp
        {
            public string Message { get;  }

            public bool Ok { get;  }

            public AssertHelp(bool ok, string message)
            {
                Ok = ok;
                Message = message;
            }
        }

        protected bool Assert(bool predicate, string message, out string value)
        {
            if (predicate)
            {
                value = null;
                return true;
            }
            value = message;
            return false;
        }

        protected bool AssertList(AssertHelp[] asserts, out string value)
        {
            value = null;
            foreach (var assert in asserts)
                if (!Assert(assert.Ok, assert.Message, out value)) return false;
            return true;
        }

        public abstract TOutput Action(TInput input);
    }
}