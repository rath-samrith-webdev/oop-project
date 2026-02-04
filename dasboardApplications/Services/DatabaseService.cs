using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scores.db");
            _connectionString = $"Data Source={dbPath}";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Scores (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        PlayerName TEXT NOT NULL,
                        GameType TEXT NOT NULL,
                        Score INTEGER NOT NULL,
                        Date TEXT NOT NULL
                    );";
                command.ExecuteNonQuery();
            }
        }

        public void SaveScore(string playerName, string gameName, int score)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Scores (PlayerName, GameType, Score, Date)
                    VALUES ($name, $game, $score, $date);";
                command.Parameters.AddWithValue("$name", playerName);
                command.Parameters.AddWithValue("$game", gameName);
                command.Parameters.AddWithValue("$score", score);
                command.Parameters.AddWithValue("$date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
            }
        }

        public List<(string Player, string Game, int Score, DateTime Date)> GetTopScores(int limit = 10)
        {
            var scores = new List<(string, string, int, DateTime)>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT PlayerName, GameType, Score, Date FROM Scores ORDER BY Score DESC LIMIT $limit;";
                command.Parameters.AddWithValue("$limit", limit);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        scores.Add((
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetInt32(2),
                            DateTime.Parse(reader.GetString(3))
                        ));
                    }
                }
            }
            return scores;
        }
    }
}
