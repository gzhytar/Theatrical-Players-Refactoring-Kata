namespace TheatricalPlayersRefactoringKata
{
    public class PerformanceInvoice
    {
        public IPlay Play { get; set; }
        public int PlayPrice { get; set; }
        public int BonusCredits { get; set; }
        public int Audience { get; set; }
    }
}