namespace TheatricalPlayersRefactoringKata
{
    public class TragedyPlay : Play
    {
        public TragedyPlay(string playName) : base("tragedy", playName)
        {
        }

        public override int CalculatePerformanceBonus(Performance perf)
        {
            int thisAmount = 40000;
            if (perf.Audience > 30)
            {
                thisAmount += 1000 * (perf.Audience - 30);
            }

            return thisAmount;
        }

        public override int CalculateVolumeCredits(Performance perf)
        {
            return 0;
        }
    }

}