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
    public class UserRepository : IRepository<User>
    {
        private readonly string _connectionString;

        public UserRepository(DatabaseService dbService)
        {
            _connectionString = dbService.GetConnectionString();
        }

        public User GetById(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Username, PasswordHash, Salt, FullName, Email, Role, FailedLoginAttempts, LockoutEnd, CreatedAt, LastLogin FROM Users WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) return MapReaderToUser(reader);
                }
            }
            return null;
        }

        public IEnumerable<User> GetAll()
        {
            var users = new List<User>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Username, PasswordHash, Salt, FullName, Email, Role, FailedLoginAttempts, LockoutEnd, CreatedAt, LastLogin FROM Users";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) users.Add(MapReaderToUser(reader));
                }
            }
            return users;
        }

        public IEnumerable<User> Find(Expression<Func<User, bool>> predicate)
        {
            return GetAll();
        }

        public int Add(User entity)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Users (Username, PasswordHash, Salt, FullName, Email, Role, FailedLoginAttempts, CreatedAt)
                    VALUES ($username, $hash, $salt, $name, $email, $role, 0, $now);
                    SELECT last_insert_rowid();";
                command.Parameters.AddWithValue("$username", entity.Username);
                command.Parameters.AddWithValue("$hash", entity.PasswordHash);
                command.Parameters.AddWithValue("$salt", entity.Salt);
                command.Parameters.AddWithValue("$name", entity.FullName ?? "");
                command.Parameters.AddWithValue("$email", entity.Email ?? "");
                command.Parameters.AddWithValue("$role", entity.Role.ToString());
                command.Parameters.AddWithValue("$now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(User entity)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Users SET
                        PasswordHash = $hash, Salt = $salt, FullName = $name, Email = $email,
                        Role = $role, FailedLoginAttempts = $attempts, LockoutEnd = $lockout,
                        LastLogin = $lastLogin, UpdatedAt = $updatedAt
                    WHERE Id = $id";
                command.Parameters.AddWithValue("$id", entity.Id);
                command.Parameters.AddWithValue("$hash", entity.PasswordHash);
                command.Parameters.AddWithValue("$salt", entity.Salt);
                command.Parameters.AddWithValue("$name", entity.FullName ?? "");
                command.Parameters.AddWithValue("$email", entity.Email ?? "");
                command.Parameters.AddWithValue("$role", entity.Role.ToString());
                command.Parameters.AddWithValue("$attempts", entity.FailedLoginAttempts);
                command.Parameters.AddWithValue("$lockout", entity.LockoutEnd?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("$lastLogin", entity.LastLogin?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("$updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Users WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();
            }
        }

        private User MapReaderToUser(SqliteDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                Salt = reader.GetString(3),
                FullName = reader.IsDBNull(4) ? "" : reader.GetString(4),
                Email = reader.IsDBNull(5) ? "" : reader.GetString(5),
                Role = Enum.Parse<UserRole>(reader.GetString(6)),
                FailedLoginAttempts = reader.GetInt32(7),
                LockoutEnd = reader.IsDBNull(8) ? (DateTime?)null : DateTime.Parse(reader.GetString(8)),
                CreatedAt = DateTime.Parse(reader.GetString(9)),
                LastLogin = reader.IsDBNull(10) ? (DateTime?)null : DateTime.Parse(reader.GetString(10))
            };
        }
    }
}
