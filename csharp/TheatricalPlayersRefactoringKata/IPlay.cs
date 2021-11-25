namespace TheatricalPlayersRefactoringKata
{
    public interface IPlay
    {
        string Name { get; }
        string Type { get; }

        public int CalculatePerformanceBonus(Performance perf);
        public int CalculateBaseCredits(Performance perf);
        public int CalculateVolumeCredits(Performance perf);
    }

}