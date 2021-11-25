using System;

namespace TheatricalPlayersRefactoringKata
{
    public abstract class Play : IPlay
    {
        private readonly string _type;
        private readonly string _name;
        public string Name { get => _name; }
        public string Type { get => _type; }

        public Play(string playType, string playName)
        {
            _type = playType;
            _name = playName;
        }

        public abstract int CalculatePerformanceBonus(Performance perf);
        public int CalculateBaseCredits(Performance perf)
        {
            return Math.Max(perf.Audience - 30, 0);
        }
        public abstract int CalculateVolumeCredits(Performance perf);
    }

}