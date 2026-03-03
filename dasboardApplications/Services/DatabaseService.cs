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

        public string GetConnectionString() => _connectionString;

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
                    );

                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        PasswordHash TEXT NOT NULL,
                        Salt TEXT NOT NULL,
                        Role TEXT NOT NULL,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS Customers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FullName TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        PhoneNumber TEXT NOT NULL,
                        Address TEXT,
                        KycDocuments TEXT,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS Loans (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CustomerId INTEGER NOT NULL,
                        LoanAmount REAL NOT NULL,
                        AnnualInterestRate REAL NOT NULL,
                        TenureInMonths INTEGER NOT NULL,
                        Type TEXT NOT NULL,
                        Frequency TEXT NOT NULL,
                        StartDate TEXT NOT NULL,
                        EndDate TEXT NOT NULL,
                        Status TEXT NOT NULL,
                        OutstandingBalance REAL NOT NULL,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT NOT NULL,
                        FOREIGN KEY(CustomerId) REFERENCES Customers(Id)
                    );

                    CREATE TABLE IF NOT EXISTS Payments (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        LoanId INTEGER NOT NULL,
                        PaymentDate TEXT NOT NULL,
                        AmountPaid REAL NOT NULL,
                        PaymentType TEXT NOT NULL,
                        Status TEXT NOT NULL,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT NOT NULL,
                        FOREIGN KEY(LoanId) REFERENCES Loans(Id)
                    );

                    CREATE TABLE IF NOT EXISTS AuditLogs (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        Action TEXT NOT NULL,
                        EntityName TEXT NOT NULL,
                        EntityId INTEGER NOT NULL,
                        Changes TEXT,
                        Timestamp TEXT NOT NULL
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
