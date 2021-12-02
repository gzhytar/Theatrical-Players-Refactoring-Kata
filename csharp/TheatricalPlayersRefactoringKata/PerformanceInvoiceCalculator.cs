using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheatricalPlayersRefactoringKata
{
    public class PerformanceInvoiceCalculator
    {
        public IList<PerformanceInvoice> CalculatePerformanceInvoices(List<Performance> performances,
            Dictionary<string, IPlay> plays)
        {
            var perfInvoices = new List<PerformanceInvoice>();

            foreach (var perf in performances)
            {
                var play = plays[perf.PlayID];

                perfInvoices.Add(new PerformanceInvoice
                {
                    Play = play,
                    PlayPrice = play.CalculatePerformanceBonus(perf),
                    BonusCredits = CalculateBonusCredits(perf, play),
                    Audience = perf.Audience
                });
            }

            return perfInvoices;
        }

        private static int CalculateBonusCredits(Performance perf, IPlay play)
        {
            var bonusCredits = play.CalculateBaseCredits(perf);
            // add extra credit for every ten comedy attendees
            bonusCredits += play.CalculateVolumeCredits(perf);

            return bonusCredits;
        }
    }
}
