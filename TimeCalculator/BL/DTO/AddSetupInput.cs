using System.Collections.Generic;

namespace TimeCalculator.BL
{
    public class AddSetupInput
    {
        public List<RunTimeCalculatorItem> Items { get; }

        public AddSetupInput(IEnumerable<RunTimeCalculatorItem> items)
        {
            Items = new List<RunTimeCalculatorItem>(items);
        }
    }
}