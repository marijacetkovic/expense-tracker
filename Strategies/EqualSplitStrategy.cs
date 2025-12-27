using ExpenseTracker.Domain.Entities;

public class EqualSplitStrategy : ISplitStrategy
{
    public List<decimal> CalculateShares(decimal totalAmount, int participantCount)
    {
        var shares = new List<decimal>();
        decimal standardShare = Math.Round(totalAmount / participantCount, 2);
        
        for (int i = 0; i < participantCount; i++) shares.Add(standardShare);

        decimal currentSum = shares.Sum();
        if (currentSum != totalAmount)
        {
            shares[0] += totalAmount - currentSum; 
        }
        
        return shares;
    }
}