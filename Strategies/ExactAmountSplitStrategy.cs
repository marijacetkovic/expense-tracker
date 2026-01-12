namespace ExpenseTracker.Strategies;

public class ExactAmountSplitStrategy : ISplitStrategy
{
    public List<decimal> CalculateShares(decimal totalAmount, int participantCount, List<decimal>? values = null)
    {
        return new List<decimal>(values);
    }


    public string? Validate(decimal totalAmount, int participantCount, List<decimal>? values = null)
    {
    
        if (values == null || values.Count != participantCount)
            return "Exact amounts must be provided for all participants.";

        if (values.Sum() != totalAmount)
            return "Exact amounts must sum to total amount.";

        return null;
    }
}
