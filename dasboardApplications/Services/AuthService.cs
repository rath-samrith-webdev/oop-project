using System;
using System.Security.Cryptography;
using Microsoft.Data.Sqlite;
using dasboardApplications.Models;

namespace dasboardApplications.Services
{
    public class AuthService
    {
        private readonly string _connectionString;
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public static User? CurrentUser { get; private set; }

        public AuthService(DatabaseService databaseService)
        {
            _connectionString = databaseService.GetConnectionString();
        }

        public bool Register(string username, string password, UserRole role)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // Check if user exists
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = $username";
                checkCmd.Parameters.AddWithValue("$username", username);
                if ((long)checkCmd.ExecuteScalar() > 0) return false;

                // Create salt and hash
                byte[] salt = new byte[SaltSize];
                RandomNumberGenerator.Create().GetBytes(salt);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
                byte[] hash = pbkdf2.GetBytes(HashSize);

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Users (Username, PasswordHash, Salt, Role, CreatedAt, UpdatedAt)
                    VALUES ($username, $hash, $salt, $role, $now, $now);";
                command.Parameters.AddWithValue("$username", username);
                command.Parameters.AddWithValue("$hash", Convert.ToBase64String(hash));
                command.Parameters.AddWithValue("$salt", Convert.ToBase64String(salt));
                command.Parameters.AddWithValue("$role", role.ToString());
                command.Parameters.AddWithValue("$now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Login(string username, string password)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Username, PasswordHash, Salt, Role FROM Users WHERE Username = $username";
                command.Parameters.AddWithValue("$username", username);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        byte[] salt = Convert.FromBase64String(reader.GetString(3));
                        byte[] storedHash = Convert.FromBase64String(reader.GetString(2));

                        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
                        byte[] hash = pbkdf2.GetBytes(HashSize);

                        bool match = true;
                        for (int i = 0; i < HashSize; i++)
                        {
                            if (hash[i] != storedHash[i]) match = false;
                        }

                        if (match)
                        {
                            CurrentUser = new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Role = Enum.Parse<UserRole>(reader.GetString(4))
                            };
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Logout() => CurrentUser = null;
    }
}
