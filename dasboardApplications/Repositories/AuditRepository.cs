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
    public class AuditRepository : IRepository<AuditLog>
    {
        private readonly string _connectionString;

        public AuditRepository(DatabaseService dbService)
        {
            _connectionString = dbService.GetConnectionString();
        }

        public AuditLog GetById(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM AuditLogs WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) return MapReaderToAuditLog(reader);
                }
            }
            return null;
        }

        public IEnumerable<AuditLog> GetAll()
        {
            var logs = new List<AuditLog>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM AuditLogs ORDER BY Timestamp DESC";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) logs.Add(MapReaderToAuditLog(reader));
                }
            }
            return logs;
        }

        public IEnumerable<AuditLog> Find(Expression<Func<AuditLog, bool>> predicate)
        {
            return GetAll();
        }

        public int Add(AuditLog entity)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO AuditLogs (UserId, Action, EntityName, EntityId, Changes, Timestamp)
                    VALUES ($userId, $action, $entityName, $entityId, $changes, $timestamp);
                    SELECT last_insert_rowid();";

                command.Parameters.AddWithValue("$userId", entity.UserId);
                command.Parameters.AddWithValue("$action", entity.Action);
                command.Parameters.AddWithValue("$entityName", entity.EntityName);
                command.Parameters.AddWithValue("$entityId", entity.EntityId);
                command.Parameters.AddWithValue("$changes", entity.Changes ?? "");
                command.Parameters.AddWithValue("$timestamp", entity.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(AuditLog entity)
        {
            // Usually audit logs are immutable, but for interface compliance:
            throw new NotImplementedException("Audit logs cannot be updated.");
        }

        public void Delete(int id)
        {
            throw new NotImplementedException("Audit logs cannot be deleted.");
        }

        private AuditLog MapReaderToAuditLog(SqliteDataReader reader)
        {
            return new AuditLog
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                Action = reader.GetString(2),
                EntityName = reader.GetString(3),
                EntityId = reader.GetInt32(4),
                Changes = reader.IsDBNull(5) ? "" : reader.GetString(5),
                Timestamp = DateTime.Parse(reader.GetString(6))
            };
        }
    }
}
