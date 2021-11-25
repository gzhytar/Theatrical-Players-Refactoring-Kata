using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string PrintAsText(Invoice invoice, Dictionary<string, Play> plays)
        {
            return GenerateReceiptWithFormat(invoice, plays, format: "text");
        }

        private static string GenerateReceiptWithFormat(Invoice invoice, Dictionary<string, Play> plays, string format)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            CultureInfo cultureInfo = new CultureInfo("en-US");

            string result = PrintStatementHeader(invoice, format);
            result += PrintStatementLines(invoice, plays, ref totalAmount, ref volumeCredits, cultureInfo, format);
            result += PrintOwnedAmount(totalAmount, cultureInfo, format);
            result += PrintOwnedCredits(volumeCredits, format);

            switch (format)
            {
                case "html":
                    result += "</html>";
                    break;
                default:
                    break;
            }

            return result;
        }

        public string PrintAsHtml(Invoice invoice, Dictionary<string, Play> plays)
        {
            return GenerateReceiptWithFormat(invoice, plays, format: "html");
        }

        private static string PrintStatementHeader(Invoice invoice, string format = "text")
        {
            var header = "";

            switch (format)
            {
                case "html": 
                    header += string.Format("<html>\n");
                    header += string.Format("<h1>Statement for {0}</h1>\n", invoice.Customer);
                    break;
                case "text":
                    header += string.Format("Statement for {0}\n", invoice.Customer);
                    break;
                default:
                    throw new ArgumentException("Incorrect input format");
            };

            return header;
        }
        
        private static string PrintStatementLines(Invoice invoice, Dictionary<string, Play> plays, ref int totalAmount, ref int volumeCredits, CultureInfo cultureInfo, string format)
        {
            var lines = "";

            switch (format)
            {
                case "html":
                    lines += "<table>\n";
                    lines += "<tr><th>play</th><th>seats</th><th>cost</th></tr>\n";
                    break;
                default:
                    break;
            }

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
                lines += PrintOrderLine(cultureInfo, perf, play, thisAmount, format);
                totalAmount += thisAmount;
            }

            switch (format)
            {
                case "html":
                    lines += "</table>\n";
                    break;
                default:
                    break;
            }
            
            return lines;
        }

        private static string PrintOwnedAmount(int totalAmount, CultureInfo cultureInfo, string format)
        {
            var result = "";
            switch (format)
            {
                case "html":
                    result += string.Format(cultureInfo, "<p>Amount owed is <em>{0:C}</em></p>\n", Convert.ToDecimal(totalAmount / 100));
                    break;
                case "text":
                    result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
                    break ;
                default:
                    break;
            }

            return result;
            
        }

        private static string PrintOwnedCredits(int volumeCredits, string format)
        {
            var result = "";
            switch (format)
            {
                case "html":
                    result += string.Format("<p>You earned <em>{0}</em> credits</p>\n", volumeCredits);
                    break;
                case "text":
                    result += string.Format("You earned {0} credits\n", volumeCredits);
                    break;
                default:
                    break;
            }

            return result;
        }

        private static string PrintOrderLine(CultureInfo cultureInfo, Performance perf, Play play, int thisAmount, string format = "text")
        {
            return format switch
            {
                "text" => string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience),
                "html" => string.Format(cultureInfo, "<tr><td>{0}</td><td>{2}</td><td>{1:C}</td></tr>\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience),
                _ => throw new ArgumentException("Incorrect input format"),
            };
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
