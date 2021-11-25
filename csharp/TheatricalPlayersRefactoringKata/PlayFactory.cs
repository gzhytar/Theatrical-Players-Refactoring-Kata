using System;

namespace TheatricalPlayersRefactoringKata
{
    public static class PlayFactory
    {
        public static Play GetPlay(string playName, string playType ) {
            switch (playType)
            {
                case "tragedy":
                    return new TragedyPlay(playName);
                case "comedy":
                    return new ComedyPlay(playName);
                default:
                    throw new ArgumentException("Invalid playType");
            }
        }
    }
}
