using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string PrintAsText(Invoice invoice, Dictionary<string, IPlay> plays)
        {
            return GenerateReceiptWithFormat(invoice, plays, format: "text");
        }
        public string PrintAsHtml(Invoice invoice, Dictionary<string, IPlay> plays)
        {
            return GenerateReceiptWithFormat(invoice, plays, format: "html");
        }

        private static string GenerateReceiptWithFormat(Invoice invoice, Dictionary<string, IPlay> plays, string format)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            CultureInfo cultureInfo = new CultureInfo("en-US");

            string result = PrintStatementHeader(invoice, format);
            result += CalculateAndPrintStatementLines(invoice, plays, ref totalAmount, ref volumeCredits, cultureInfo, format);
            result += PrintOwnedAmount(totalAmount, cultureInfo, format);
            result += PrintOwnedCredits(volumeCredits, format);
            result += PrintStatementFooter(format);

            return result;
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

        private static string PrintStatementFooter(string format)
        {
            var result = "";
           
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

        private static string CalculateAndPrintStatementLines(Invoice invoice, Dictionary<string, IPlay> plays, ref int totalAmount, ref int volumeCredits, CultureInfo cultureInfo, string format)
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
                var amount = play.CalculatePerformanceBonus(perf);
                // add volume credits
                volumeCredits += play.CalculateBaseCredits(perf);
                // add extra credit for every ten comedy attendees
                volumeCredits += play.CalculateVolumeCredits(perf);

                // print line for this order
                lines += PrintOrderLine(cultureInfo, perf, play, amount, format);
                totalAmount += amount;
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

        private static string PrintOwnedAmount(int amount, CultureInfo cultureInfo, string format)
        {
            var result = "";
            switch (format)
            {
                case "html":
                    result += string.Format(cultureInfo, "<p>Amount owed is <em>{0:C}</em></p>\n", Convert.ToDecimal(amount / 100));
                    break;
                case "text":
                    result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(amount / 100));
                    break ;
                default:
                    break;
            }

            return result;
            
        }

        private static string PrintOwnedCredits(int credits, string format)
        {
            var result = "";
            switch (format)
            {
                case "html":
                    result += string.Format("<p>You earned <em>{0}</em> credits</p>\n", credits);
                    break;
                case "text":
                    result += string.Format("You earned {0} credits\n", credits);
                    break;
                default:
                    break;
            }

            return result;
        }

        private static string PrintOrderLine(CultureInfo cultureInfo, Performance perf, IPlay play, int amount, string format = "text")
        {
            return format switch
            {
                "text" => string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(amount / 100), perf.Audience),
                "html" => string.Format(cultureInfo, "<tr><td>{0}</td><td>{2}</td><td>{1:C}</td></tr>\n", play.Name, Convert.ToDecimal(amount / 100), perf.Audience),
                _ => throw new ArgumentException("Incorrect input format"),
            };
        }
    }
}
