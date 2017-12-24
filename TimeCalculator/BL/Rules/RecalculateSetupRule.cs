using System.Linq;
using Microsoft.EntityFrameworkCore;
using TimeCalculator.Data;

namespace TimeCalculator.BL.Rules
{
    public sealed class RecalculateSetupRule : ISimpleRule<object, object>
    {
        public object Action(object input)
        {
            using (var database = DataBaseFactory.CreateDatabase())
            {
                foreach (var group in database.SetupEntities.AsNoTracking().GroupBy(e => e.SetupType))
                {
                    var time = group.Average(e => e.Value);

                    switch (group.Key)
                    {
                        case SetupType.Setup:
                            Properties.Settings.Default.SetupTime = (int) time;
                            break;
                        case SetupType.Iteration:
                            Properties.Settings.Default.IterationTime = (int) time;
                            break;
                    }
                }
            }

            Properties.Settings.Default.Save();

            return null;
        }
    }
}