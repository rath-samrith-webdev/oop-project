using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Data.Sqlite;
using dasboardApplications.Interfaces;
using dasboardApplications.Models;
using dasboardApplications.Core;
using dasboardApplications.Services;

namespace dasboardApplications.Repositories
{
    public class ScoreRepository : IRepository<ScoreRecord>
    {
        private readonly string _connectionString;

        public ScoreRepository(DatabaseService dbService)
        {
            _connectionString = dbService.GetConnectionString();
        }

        public ScoreRecord GetById(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Scores WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) return MapReaderToScore(reader);
                }
            }
            return null;
        }

        public IEnumerable<ScoreRecord> GetAll()
        {
            var scores = new List<ScoreRecord>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Scores ORDER BY Score DESC";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) scores.Add(MapReaderToScore(reader));
                }
            }
            return scores;
        }

        public IEnumerable<ScoreRecord> Find(Expression<Func<ScoreRecord, bool>> predicate)
        {
            return GetAll();
        }

        public int Add(ScoreRecord entity)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Scores (PlayerName, GameType, Score, Date)
                    VALUES ($name, $game, $score, $date);
                    SELECT last_insert_rowid();";
                command.Parameters.AddWithValue("$name", entity.PlayerName);
                command.Parameters.AddWithValue("$game", entity.GameType);
                command.Parameters.AddWithValue("$score", entity.Score);
                command.Parameters.AddWithValue("$date", entity.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(ScoreRecord entity) { }
        public void Delete(int id) { }

        private ScoreRecord MapReaderToScore(SqliteDataReader reader)
        {
            return new ScoreRecord
            {
                Id = reader.GetInt32(0),
                PlayerName = reader.GetString(1),
                GameType = reader.GetString(2),
                Score = reader.GetInt32(3),
                Date = DateTime.Parse(reader.GetString(4))
            };
        }
    }
}
