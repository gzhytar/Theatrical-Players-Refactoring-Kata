using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string PrintAsText(Invoice invoice, Dictionary<string, IPlay> plays)
        {
            var performanceInvoices = new PerformanceInvoiceCalculator().CalculatePerformanceInvoices(invoice.Performances, plays);

            return PrintAs(invoice, performanceInvoices, format: "text");
        }
        public string PrintAsHtml(Invoice invoice, Dictionary<string, IPlay> plays)
        {
            var performanceInvoices = new PerformanceInvoiceCalculator().CalculatePerformanceInvoices(invoice.Performances, plays);

            return PrintAs(invoice, performanceInvoices, format: "html");
        }

        private static string PrintAs(Invoice invoice, IList<PerformanceInvoice> perfInvoices, string format)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");

            string result = PrintReceiptHeader(invoice.Customer, format);
            result += PrintDetailsHeader(format);

            var totalAmount = perfInvoices.Sum(x => x.PlayPrice);
            var totalCredits = perfInvoices.Sum(x => x.BonusCredits);

            foreach (var playInvoice in perfInvoices)
            {
                // print line for this order
                result += PrintDetailsLine(cultureInfo, playInvoice.PlayPrice, playInvoice.Play.Name, playInvoice.Audience, format);
            }

            result += PrintDetailsFooter(format);
            result += PrintOwnedAmount(totalAmount, cultureInfo, format);
            result += PrintOwnedCredits(totalCredits, format);
            result += PrintReceiptFooter(format);

            return result;
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
