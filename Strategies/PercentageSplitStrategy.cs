namespace ExpenseTracker.Strategies;

public class PercentageSplitStrategy : ISplitStrategy
{
    public List<decimal> CalculateShares(decimal totalAmount, int participantCount, List<decimal>? values = null)
    {
        var rawAmounts = new List<decimal>();
        for (int i = 0; i < participantCount; i++)
        {
            rawAmounts.Add(totalAmount * (values[i] / 100m));
        }

        return RoundingHelper.ApplyRounding(rawAmounts, totalAmount, 0);
    }
    public string? Validate(decimal totalAmount, int participantCount, List<decimal>? values = null)
    {
        if (values == null || values.Count != participantCount)
            return "Percentages must be provided for all participants.";

        if (values.Sum() != 100)
            return "Percentages must sum to 100.";

        return null;
    }
}

