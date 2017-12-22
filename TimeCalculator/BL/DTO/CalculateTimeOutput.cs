namespace TimeCalculator.BL
{
    public sealed class CalculateTimeOutput
    {
        public int? SetupTime { get; }
        public int? IterationTime { get; }
        public int? RunTime { get; }
        
        public string Error { get; }
        public PrecisionMode PrecisionMode { get; }


        public CalculateTimeOutput(int? setupTime, int? iterationTime, int? runTime, string error, PrecisionMode precisionMode)
        {
            SetupTime = setupTime;
            IterationTime = iterationTime;
            RunTime = runTime;
            Error = error;
            PrecisionMode = precisionMode;
        }
    }
}