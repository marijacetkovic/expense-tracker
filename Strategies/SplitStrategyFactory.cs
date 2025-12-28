namespace ExpenseTracker.Strategies;
public static class SplitStrategyFactory
{
    public static ISplitStrategy Create(SplitType type) => type switch
    {
        SplitType.Equal => new EqualSplitStrategy(),
        //add new stragegies here
        _ => throw new NotSupportedException()
    };
}
