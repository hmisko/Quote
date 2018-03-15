namespace Quoting.Strategies
{
    public interface IRepaymentCalculationStrategy
    {
        decimal Calculate(decimal requestedAmount, decimal rate, int numberOfMonths);
    }
}