public enum SplitType { Equal }
public interface ISplitStrategy
{
    List<decimal> CalculateShares(decimal totalAmount, int participantCount);
}