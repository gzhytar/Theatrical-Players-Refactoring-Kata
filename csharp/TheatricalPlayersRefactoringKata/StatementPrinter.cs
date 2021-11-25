using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string PrintAsText(Invoice invoice, Dictionary<string, IPlay> plays)
        {
            return PrintAs(invoice, plays, format: "text");
        }
        public string PrintAsHtml(Invoice invoice, Dictionary<string, IPlay> plays)
        {
            return PrintAs(invoice, plays, format: "html");
        }

        private static string PrintAs(Invoice invoice, Dictionary<string, IPlay> plays, string format)
        {
            var totalAmount = 0;
            var totalCredits = 0;
            CultureInfo cultureInfo = new CultureInfo("en-US");

            string result = PrintReceiptHeader(invoice.Customer, format);
            result += PrintDetailsHeader(format);

            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];
                int lineAmount, lineCredits;
                CalculateLineTotals(perf, play, out lineAmount, out lineCredits);

                // print line for this order
                result += PrintDetailsLine(cultureInfo, lineAmount, play.Name, perf.Audience, format);

                RecalculateInvoiceTotals(lineAmount, lineCredits, out totalAmount, out totalCredits);
            }

            result += PrintDetailsFooter(format);
            result += PrintOwnedAmount(totalAmount, cultureInfo, format);
            result += PrintOwnedCredits(totalCredits, format);
            result += PrintReceiptFooter(format);

            return result;
        }

        private static void CalculateLineTotals(Performance perf, IPlay play, out int lineAmount, out int lineCredits)
        {
            lineAmount = play.CalculatePerformanceBonus(perf);
            // calculate volume credits
            lineCredits = play.CalculateBaseCredits(perf);
            // add extra credit for every ten comedy attendees
            lineCredits += play.CalculateVolumeCredits(perf);
        }

        private static string PrintReceiptHeader(string customer, string format = "text")
        {
            var header = "";
            switch (format)
            {
                case "html": 
                    header += string.Format("<html>\n");
                    header += string.Format("<h1>Statement for {0}</h1>\n", customer);
                    break;
                case "text":
                    header += string.Format("Statement for {0}\n", customer);
                    break;
                default:
                    throw new ArgumentException("Incorrect input format");
            };

            return header;
        }

        private static string PrintReceiptFooter(string format)
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

        private static void RecalculateInvoiceTotals(int amount, int credits, out int totalAmount, out int totalCredits)
        {
            totalAmount += amount;
            totalCredits += credits;
        }

        private static string PrintDetailsFooter(string format)
        {
            var lines = "";
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

        private static string PrintDetailsHeader(string format)
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

        private static string PrintDetailsLine(CultureInfo cultureInfo, int amount, string name, int audience, string format = "text")
        {
            return format switch
            {
                "text" => string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", name, Convert.ToDecimal(amount / 100), audience),
                "html" => string.Format(cultureInfo, "<tr><td>{0}</td><td>{2}</td><td>{1:C}</td></tr>\n", name, Convert.ToDecimal(amount / 100), audience),
                _ => throw new ArgumentException("Incorrect input format"),
            };
        }
    }
}
