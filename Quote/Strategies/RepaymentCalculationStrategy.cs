namespace Quoting.Strategies
{
    using System;

    public class RepaymentCalculationStrategy : IRepaymentCalculationStrategy
    {
        public decimal Calculate(decimal requestedAmount, decimal rate, int numberOfMonths)
        {
            var r = Math.Pow(1 + (double) rate, 1 / 12d) - 1;
            var result = r * (double) requestedAmount /
                         (1 - Math.Pow(1 + r, -numberOfMonths));

            return (decimal)result;
        }
    }
}