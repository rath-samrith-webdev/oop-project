using System;

namespace dasboardApplications.Models
{
    public class ScoreRecord
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public string GameType { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
    }
}
