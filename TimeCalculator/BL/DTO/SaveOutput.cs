namespace TimeCalculator.BL
{
    public class SaveOutput
    {
        public bool Succsess { get; }
        public string Message { get; }


        public SaveOutput(bool succsess, string message)
        {
            Succsess = succsess;
            Message = message;
        }
    }
}