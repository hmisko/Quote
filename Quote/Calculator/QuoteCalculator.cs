namespace Quoting.Calculator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Quoting.Strategies;

    public class QuoteCalculator : IQuoteCalculator
    {
        private readonly IRepaymentCalculationStrategy repaymentCalculationStrategy;

        public QuoteCalculator(IRepaymentCalculationStrategy repaymentCalculationStrategy)
        {
            this.repaymentCalculationStrategy = repaymentCalculationStrategy;
        }

        public Quote Calculate(IEnumerable<LenderOffer> lenderOffers, decimal requestedAmount, int numberOfMonths)
        {
            var sortedOffers = lenderOffers.OrderBy(x => x.Rate);

            var loanAmount = 0m;
            var averageRate = 0m;
            var monthlyRepayment = 0m;

            foreach (var offer in sortedOffers)
            {
                var nextPartialLoan = Math.Min(requestedAmount - loanAmount, offer.Available);
                monthlyRepayment += this.repaymentCalculationStrategy.Calculate(nextPartialLoan, offer.Rate, numberOfMonths);
                averageRate = (averageRate * loanAmount + offer.Rate * nextPartialLoan) /
                              (loanAmount + nextPartialLoan);
                loanAmount += nextPartialLoan;

                if (loanAmount == requestedAmount)
                {
                    break;
                }
            }

            if (loanAmount < requestedAmount)
            {
                return null;
            }

            return new Quote
                   {
                       MonthlyRepayment = monthlyRepayment,
                       TotalRepayment = monthlyRepayment * numberOfMonths,
                       Rate = averageRate
                   };
        }
    }
}