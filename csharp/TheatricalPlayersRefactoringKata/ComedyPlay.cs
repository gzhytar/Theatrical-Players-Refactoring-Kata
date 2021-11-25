using System;

namespace TheatricalPlayersRefactoringKata
{
    public class ComedyPlay : Play
    {
        public ComedyPlay(string playName) : base("comedy", playName)
        {
        }

        public override int CalculatePerformanceBonus(Performance perf)
        {
            int thisAmount = 30000;
            if (perf.Audience > 20)
            {
                thisAmount += 10000 + 500 * (perf.Audience - 20);
            }
            thisAmount += 300 * perf.Audience;

            return thisAmount;
        }

        public override int CalculateVolumeCredits(Performance perf)
        {
            return (int)Math.Floor((decimal)perf.Audience / 5);
        }
    }

}