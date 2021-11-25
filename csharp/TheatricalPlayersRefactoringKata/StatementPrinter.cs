using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            CultureInfo cultureInfo = new CultureInfo("en-US");

            string result = PrintStatementHeader(invoice);
            result += PrintStatementLines(invoice, plays, ref totalAmount, ref volumeCredits, cultureInfo);
            result += PrintOwnedAmount(totalAmount, cultureInfo);
            result = PrintOwnedCredits(volumeCredits, result);

            return result;
        }

        private static string PrintStatementLines(Invoice invoice, Dictionary<string, Play> plays, ref int totalAmount, ref int volumeCredits, CultureInfo cultureInfo)
        {
            var lines = "";

            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];
                var thisAmount = 0;
                thisAmount = CalculatePerformanceBonus(perf, play);
                // add volume credits
                volumeCredits += CalculateCredits(perf);
                // add extra credit for every ten comedy attendees
                volumeCredits = AddVolumeCreditsForPlayType(volumeCredits, perf, play);

                // print line for this order
                lines += PrintOrderLine(cultureInfo, perf, play, thisAmount);
                totalAmount += thisAmount;
            }

            return lines;
        }

        private static string PrintStatementHeader(Invoice invoice)
        {
            return string.Format("Statement for {0}\n", invoice.Customer);
        }

        private static string PrintOwnedCredits(int volumeCredits, string result)
        {
            result += String.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private static string PrintOwnedAmount(int totalAmount, CultureInfo cultureInfo)
        {
            return String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
        }

        private static string PrintOrderLine(CultureInfo cultureInfo, Performance perf, Play play, int thisAmount)
        {
            return String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
        }

        private static int AddVolumeCreditsForPlayType(int volumeCredits, Performance perf, Play play)
        {
            if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);
            return volumeCredits;
        }

        private static int CalculateCredits(Performance perf)
        {
            return Math.Max(perf.Audience - 30, 0);
        }

        private static int CalculatePerformanceBonus(Performance perf, Play play)
        {
            int thisAmount;
            switch (play.Type)
            {
                case "tragedy":
                    thisAmount = 40000;
                    if (perf.Audience > 30)
                    {
                        thisAmount += 1000 * (perf.Audience - 30);
                    }
                    break;
                case "comedy":
                    thisAmount = 30000;
                    if (perf.Audience > 20)
                    {
                        thisAmount += 10000 + 500 * (perf.Audience - 20);
                    }
                    thisAmount += 300 * perf.Audience;
                    break;
                default:
                    throw new Exception("unknown type: " + play.Type);
            }

            return thisAmount;
        }
    }
}
