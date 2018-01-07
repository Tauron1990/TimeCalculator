using System;
using System.Collections.Generic;
using TimeCalculator.Charts.DTO;

namespace TimeCalculator.Charts.Builder.Controls
{
    public static class TestData
    {
        public static AvarageSetupTimePerQuaterBuilder AvarageSetupTimePerQuater => new AvarageSetupTimePerQuaterBuilder(new List<AvarageSetupTimePerQuater>
        {
            new AvarageSetupTimePerQuater(new DateTime(2018,1,1), TimeSpan.FromMinutes(20).TotalMilliseconds, TimeSpan.FromMinutes(25 * 0.7d).TotalMilliseconds, TimeSpan.FromMinutes(10 * 1.2d).TotalMilliseconds, TimeSpan.FromMinutes(30 * 1.5d).TotalMilliseconds),
            new AvarageSetupTimePerQuater(new DateTime(2019,1,1), TimeSpan.FromMinutes(30).TotalMilliseconds, TimeSpan.FromMinutes(14 * 0.7d).TotalMilliseconds, TimeSpan.FromMinutes(13 * 1.2d).TotalMilliseconds, TimeSpan.FromMinutes(27 * 1.5d).TotalMilliseconds),
            new AvarageSetupTimePerQuater(new DateTime(2020,1,1), TimeSpan.FromMinutes(17).TotalMilliseconds, TimeSpan.FromMinutes(22 * 0.7d).TotalMilliseconds, TimeSpan.FromMinutes(34 * 1.2d).TotalMilliseconds, TimeSpan.FromMinutes(15 * 1.5d).TotalMilliseconds),
            new AvarageSetupTimePerQuater(new DateTime(2021,1,1), TimeSpan.FromMinutes(22).TotalMilliseconds, TimeSpan.FromMinutes(28 * 0.7d).TotalMilliseconds, TimeSpan.FromMinutes(28 * 1.2d).TotalMilliseconds, TimeSpan.FromMinutes(25 * 1.5d).TotalMilliseconds),
            new AvarageSetupTimePerQuater(new DateTime(2022,1,1), TimeSpan.FromMinutes(15).TotalMilliseconds, TimeSpan.FromMinutes(32 * 0.7d).TotalMilliseconds, TimeSpan.FromMinutes(20 * 1.2d).TotalMilliseconds, TimeSpan.FromMinutes(26 * 1.5d).TotalMilliseconds)
        });
    }
}