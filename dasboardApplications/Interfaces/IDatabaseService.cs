using System;
using System.Collections.Generic;

namespace dasboardApplications.Interfaces
{
    public interface IDatabaseService
    {
        void SaveScore(string playerName, string gameName, int score);
        List<(string Player, string Game, int Score, DateTime Date)> GetTopScores(int limit = 10);
    }
}
