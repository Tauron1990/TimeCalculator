using System;
using JetBrains.Annotations;

namespace TimeCalculator.Charts.DTO
{
    public class AvarageSetupTimePerQuater
    {
        public AvarageSetupTimePerQuater(DateTime year, double quater1, double quater2, double quater3, double quater4)
        {
            Year = year;
            Quater1 = quater1;
            Quater2 = quater2;
            Quater3 = quater3;
            Quater4 = quater4;
        }

        [UsedImplicitly]
        public DateTime Year { get;  }

        [UsedImplicitly]
        public double Quater1 { get;  }

        [UsedImplicitly]
        public double Quater2 { get;  }

        [UsedImplicitly]
        public double Quater3 { get;  }

        [UsedImplicitly]
        public double Quater4 { get;  }
    }
}