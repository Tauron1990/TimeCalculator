namespace TimeCalculator.BL
{
    public sealed class CalculateTimeOutput
    {
        public int? SetupTime { get; }
        public int? IterationTime { get; }
        public int? RunTime { get; }
        public string Error { get; }

        public CalculateTimeOutput(int? setupTime, int? iterationTime, int? runTime, string error)
        {
            SetupTime = setupTime;
            IterationTime = iterationTime;
            RunTime = runTime;
            Error = error;
        }
    }
}