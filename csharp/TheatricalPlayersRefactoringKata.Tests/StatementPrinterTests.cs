using System;
using System.Collections.Generic;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;

namespace TheatricalPlayersRefactoringKata.Tests
{
    [TestFixture]
    public class StatementPrinterTests
    {
        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void test_statement_plain_text_example()
        {
            var plays = new Dictionary<string, IPlay>();
            plays.Add("hamlet", PlayFactory.GetPlay("Hamlet", "tragedy"));
            plays.Add("as-like", PlayFactory.GetPlay("As You Like It", "comedy"));
            plays.Add("othello", PlayFactory.GetPlay("Othello", "tragedy"));

            Invoice invoice = new Invoice("BigCo", new List<Performance>{new Performance("hamlet", 55),
                new Performance("as-like", 35),
                new Performance("othello", 40)});
            
            StatementPrinter statementPrinter = new StatementPrinter();
            var result = statementPrinter.PrintAsText(invoice, plays);
            
            Approvals.Verify(result);
        }

        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void test_statement_html_example()
        {
            var plays = new Dictionary<string, IPlay>();
            plays.Add("hamlet", PlayFactory.GetPlay("Hamlet", "tragedy"));
            plays.Add("as-like", PlayFactory.GetPlay("As You Like It", "comedy"));
            plays.Add("othello", PlayFactory.GetPlay("Othello", "tragedy"));

            Invoice invoice = new Invoice("BigCo", new List<Performance>{new Performance("hamlet", 55),
                new Performance("as-like", 35),
                new Performance("othello", 40)});
            
            StatementPrinter statementPrinter = new StatementPrinter();
            
             var result = statementPrinter.PrintAsHtml(invoice, plays);
             Approvals.Verify(result);
        }

        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void test_statement_with_new_play_types()
        {
            //var plays = new Dictionary<string, IPlay>();
            //plays.Add("henry-v", PlayFactory.GetPlay("Henry V", "history"));
            //plays.Add("as-like", PlayFactory.GetPlay("As You Like It", "pastoral"));

            //Invoice invoice = new Invoice("BigCoII", new List<Performance>{new Performance("henry-v", 53),
            //    new Performance("as-like", 55)});
            
            //StatementPrinter statementPrinter = new StatementPrinter();

            //Assert.Throws<Exception>(() => statementPrinter.PrintAsText(invoice, plays));
        }
    }
}
