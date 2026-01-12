namespace ExpenseTracker.Strategies;
public static class SplitStrategyFactory
{
    public static ISplitStrategy Create(SplitType type) => type switch
    {
        SplitType.Equal => new EqualSplitStrategy(),
        SplitType.Exact => new ExactAmountSplitStrategy(),
        SplitType.Percentage => new PercentageSplitStrategy(),
        //add new stragegies here
        _ => throw new NotSupportedException()
    };
}
