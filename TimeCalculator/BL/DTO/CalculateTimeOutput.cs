﻿using System;

namespace TimeCalculator.BL
{
    public sealed class CalculateTimeOutput
    {
        public TimeSpan? SetupTime { get; }
        public TimeSpan? IterationTime { get; }
        public TimeSpan? RunTime { get; }
        
        public string Error { get; }
        public PrecisionMode PrecisionMode { get; }


        public CalculateTimeOutput(TimeSpan? setupTime, TimeSpan? iterationTime, TimeSpan? runTime, string error, PrecisionMode precisionMode)
        {
            SetupTime = setupTime;
            IterationTime = iterationTime;
            RunTime = runTime;
            Error = error;
            PrecisionMode = precisionMode;
        }
    }
}