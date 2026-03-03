using System;
using Microsoft.Data.Sqlite;
using dasboardApplications.Models;

namespace dasboardApplications.Services
{
    public class AuditService
    {
        private readonly string _connectionString;

        public AuditService(DatabaseService databaseService)
        {
            _connectionString = databaseService.GetConnectionString();
        }

        public void LogAction(string action, string entityName, int entityId, string changes)
        {
            int userId = AuthService.CurrentUser?.Id ?? 0;

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO AuditLogs (UserId, Action, EntityName, EntityId, Changes, Timestamp)
                    VALUES ($userId, $action, $entityName, $entityId, $changes, $timestamp);";
                command.Parameters.AddWithValue("$userId", userId);
                command.Parameters.AddWithValue("$action", action);
                command.Parameters.AddWithValue("$entityName", entityName);
                command.Parameters.AddWithValue("$entityId", entityId);
                command.Parameters.AddWithValue("$changes", changes);
                command.Parameters.AddWithValue("$timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
            }
        }
    }
}
