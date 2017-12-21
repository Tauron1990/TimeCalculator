namespace TimeCalculator.BL
{
    public sealed class CalculateTimeInput
    {
        public long? Iterations { get; }
        public PaperFormat PaperFormat { get; }
        public long? Amount { get; }
        public double? Speed { get; }

        public CalculateTimeInput(long? iterations, PaperFormat paperFormat, long? amount, double? speed)
        {
            Iterations = iterations;
            PaperFormat = paperFormat;
            Amount = amount;
            Speed = speed;
        }
    }
}