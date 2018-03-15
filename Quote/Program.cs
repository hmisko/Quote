namespace Quoting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Quoting.Calculator;
    using Quoting.Strategies;
    using CsvHelper;
    using Microsoft.Extensions.DependencyInjection;

    class Program
    {
        private const int MinLoan = 1000;
        private const int MaxLoan = 15000;
        private const int IncrementLoan = 100;
        private const int NumberOfMonths = 36;

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: quote.exe [market_file] [loan_amount]");
                return;
            }
            var marketFile = args[0];
            if (!File.Exists(marketFile))
            {
                Console.WriteLine($"File \"{marketFile}\" not found.");
                return;
            }
            if (!decimal.TryParse(args[1], out var loanAmount))
            {
                Console.WriteLine($"Loan amount {args[1]} is not valid.");
                return;
            }
            if (loanAmount < MinLoan || loanAmount > MaxLoan || loanAmount % IncrementLoan > 0)
            {
                Console.WriteLine($"Loan should be of any £100 increment between £{MinLoan} and £{MaxLoan} inclusive.");
                return;
            }

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IRepaymentCalculationStrategy, RepaymentCalculationStrategy>()
                .AddSingleton<IQuoteCalculator, QuoteCalculator>()
                .BuildServiceProvider();

            var lenderOffers = ReadLenderOffers(marketFile);

            var quote = serviceProvider.GetService<IQuoteCalculator>()
                .Calculate(lenderOffers, loanAmount, NumberOfMonths);

            if (quote == null)
            {
                Console.WriteLine("It is not possible to provide a quote a this time.");
            }
            else
            {
                Console.WriteLine($"Requested amount: £{loanAmount}");
                Console.WriteLine($"Rate: {quote.Rate * 100:F1} %");
                Console.WriteLine($"Monthly repayment: £{quote.MonthlyRepayment:F}");
                Console.WriteLine($"Total repayment: £{quote.TotalRepayment:F}");
            }
        }

        private static IEnumerable<LenderOffer> ReadLenderOffers(string marketFile)
        {
            IEnumerable<LenderOffer> lenderOffers;

            using (var textReader = File.OpenText(marketFile))
            {
                using (var csvReader = new CsvReader(textReader))
                {
                    lenderOffers = csvReader.GetRecords<LenderOffer>().ToList();
                }
            }
            return lenderOffers;
        }
    }
}