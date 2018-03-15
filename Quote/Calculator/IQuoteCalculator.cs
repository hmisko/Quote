namespace Quoting.Calculator
{
    using System.Collections.Generic;

    public interface IQuoteCalculator
    {
        Quote Calculate(IEnumerable<LenderOffer> lenderOffers, decimal requestedAmount, int numberOfMonths);
    }
}