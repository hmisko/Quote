namespace Quoting.Tests
{
    using System;
    using NUnit.Framework;
    using Quoting.Calculator;
    using Quoting.Strategies;

    [TestFixture]
    public class QuoteCalculatorTest
    {
        private const int NumberOfMonths = 36;

        [Test]
        public void ShouldReturnAggregatedQuoteForMulipleLenderOffers()
        {
            // arrange
            var requestedAmount = 1000m;
            var lenderOffers =
                new[]
                {
                    new LenderOffer { Available = 480, Rate = 0.069m },
                    new LenderOffer { Available = 520, Rate = 0.071m },
                };
            var repaymentCalculationStrategy = new RepaymentCalculationStrategy();
            IQuoteCalculator quoteCalculator = new QuoteCalculator(repaymentCalculationStrategy);

            // act
            var resultQuote = quoteCalculator.Calculate(lenderOffers, requestedAmount, NumberOfMonths);

            // assert 
            Assert.That(Math.Round(resultQuote.Rate * 100, 1),
                        Is.EqualTo(7));
            Assert.That(Math.Round(resultQuote.MonthlyRepayment, 2),
                        Is.EqualTo(30.78m));
            Assert.That(Math.Round(resultQuote.TotalRepayment, 2),
                        Is.EqualTo(1108.10m));
        }

        [Test]
        public void ShouldChooseOffersWithTheLowestRates()
        {
            // arrange
            var requestedAmount = 1000m;
            var lenderOffers =
                new[]
                {
                    new LenderOffer { Rate = 0.075m, Available = 640 },
                    new LenderOffer { Rate = 0.069m, Available = 480 },
                    new LenderOffer { Rate = 0.071m, Available = 520 },
                    new LenderOffer { Rate = 0.104m, Available = 170 },
                    new LenderOffer { Rate = 0.081m, Available = 320 },
                    new LenderOffer { Rate = 0.074m, Available = 140 },
                    new LenderOffer { Rate = 0.071m, Available = 60 },
                };
            var repaymentCalculationStrategy = new RepaymentCalculationStrategy();
            IQuoteCalculator quoteCalculator = new QuoteCalculator(repaymentCalculationStrategy);

            // act
            var resultQuote = quoteCalculator.Calculate(lenderOffers, requestedAmount, NumberOfMonths);

            // assert 
            Assert.That(Math.Round(resultQuote.MonthlyRepayment, 2),
                        Is.EqualTo(30.78m));
            Assert.That(Math.Round(resultQuote.TotalRepayment, 2),
                        Is.EqualTo(1108.10m));
        }

        [Test]
        public void ShouldReturnNullIfNotSufficientOffers()
        {
            // arrange
            var requestedAmount = 1000m;
            var lenderOffers =
                new[]
                {
                    new LenderOffer { Rate = 0.104m, Available = 170 },
                    new LenderOffer { Rate = 0.081m, Available = 320 },
                    new LenderOffer { Rate = 0.074m, Available = 140 },
                    new LenderOffer { Rate = 0.071m, Available = 60 },
                };
            var repaymentCalculationStrategy = new RepaymentCalculationStrategy();
            IQuoteCalculator quoteCalculator = new QuoteCalculator(repaymentCalculationStrategy);

            // act
            var resultQuote = quoteCalculator.Calculate(lenderOffers, requestedAmount, NumberOfMonths);

            // assert 
            Assert.That(resultQuote, Is.Null);
        }
    }
}