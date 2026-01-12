namespace ExpenseTracker.Strategies;

public static class RoundingHelper
{
    public static List<decimal> ApplyRounding(
        List<decimal> rawAmounts,
        decimal totalAmount,
        int ownerIndex
    )
    {
        // floor amounts to cents
        var rounded = rawAmounts
            .Select(a => Math.Floor(a * 100m) / 100m)
            .ToList();

        // compute remainder
        decimal roundedSum = rounded.Sum();
        decimal remainder = totalAmount - roundedSum;

        // assign remainder to owner
        rounded[ownerIndex] += remainder;

        return rounded;
    }
}
