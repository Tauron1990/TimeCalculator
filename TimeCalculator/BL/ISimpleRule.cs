namespace TimeCalculator.BL
{
    public interface ISimpleRule<TResult, TInput>
    {
        TResult Action(TInput input);
    }
}