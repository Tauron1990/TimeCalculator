using System.Collections.Generic;
using System.Windows.Controls;
using TimeCalculator.Charts.Builder.Controls;
using TimeCalculator.Charts.DTO;

namespace TimeCalculator.Charts.Builder
{
    public sealed class AvarageSetupTimePerQuaterBuilder : ChartDatabase
    {
        public IEnumerable<AvarageSetupTimePerQuater> Items { get; }

        public AvarageSetupTimePerQuaterBuilder(IList<AvarageSetupTimePerQuater> items)
        {
            Items = items;
        }

        public override Control CreateControl()
        {
            return new AvarageSetupTimePerQuaterControl() { DataContext = this };
        }
    }
}