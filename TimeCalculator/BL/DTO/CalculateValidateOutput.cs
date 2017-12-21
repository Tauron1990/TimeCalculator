namespace TimeCalculator.BL
{
    public sealed class CalculateValidateOutput
    {
        public bool Valid { get; }
        public string Message { get; }

        public CalculateValidateOutput(bool valid, string message)
        {
            Valid = valid;
            Message = message;
        }
    }
}