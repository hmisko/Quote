namespace Quoting.Tests
{
    using System;
    using NUnit.Framework;
    using Quoting.Strategies;

    [TestFixture]
    public class RepaymentCalculationStrategyTests
    {
        [Test]
        public void ShouldReturnProperMonthlyRepayment()
        {
            // arrange
            var requestedAmount = 1000m;
            var rate = 0.07m;
            var numberOfMonths = 36;
            IRepaymentCalculationStrategy repaymentCalculationStrategy = new RepaymentCalculationStrategy();

            // act 
            var result = repaymentCalculationStrategy.Calculate(requestedAmount, rate, numberOfMonths);

            // assert 
            Assert.That(Math.Round(result, 2), Is.EqualTo(30.78m));
        }
    }
}