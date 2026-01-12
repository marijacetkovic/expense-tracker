namespace ExpenseTracker.Strategies;

public enum SplitType { Equal, Exact, Percentage }
public interface ISplitStrategy
{
    List<decimal> CalculateShares(decimal totalAmount, int participantCount, List<decimal>? values = null);
    string? Validate(decimal totalAmount, int participantCount, List<decimal>? values = null);

}