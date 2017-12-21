namespace TimeCalculator.BL
{
    public interface ISimpleRule<out TResult, in TInput>
    {
        TResult Action(TInput input);
    }
}