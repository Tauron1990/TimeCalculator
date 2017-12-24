using System;
using System.Linq;
using TimeCalculator.Data;

namespace TimeCalculator.BL.Rules
{
    public sealed class AddSetupItemRule : ISimpleRule<object, AddSetupInput>
    {
        public object Action(AddSetupInput input)
        {
            using (var database = DataBaseFactory.CreateDatabase())
            {
                foreach (var item in input.Items.Where(i => i != null))
                    AddSetupEntity(database, item);
                database.SaveChanges();
            }

            return null;
        }

        private void AddSetupEntity(JobDatabase database, RunTimeCalculatorItem item)
        {
            SetupType type;
            switch (item.ItemType)
            {
                case RunTimeCalculatorItemType.Iteration:
                    type = SetupType.Iteration;
                    break;
                case RunTimeCalculatorItemType.Setup:
                    type = SetupType.Setup;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var time = item.CalculateDiffernce();
            if(time == null) return;

            database.SetupEntities.Add(new SetupEntity {SetupType = type, Value = (int) time.Value.TotalMinutes, StartTime = item.StartTime});
        }
    }
}