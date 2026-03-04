using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Data.Sqlite;
using dasboardApplications.Models;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Services
{
    public class AuthService
    {
        private readonly IRepository<User> _userRepository;
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;
        private const int MaxFailedAttempts = 5;
        private const int LockoutMinutes = 15;

        public static User? CurrentUser { get; private set; }

        public AuthService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public bool ValidatePasswordStrength(string password, out string message)
        {
            if (password.Length < 8)
            {
                message = "Password must be at least 8 characters long.";
                return false;
            }
            if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
            {
                message = "Password must contain uppercase, lowercase, and numeric characters.";
                return false;
            }
            message = "Strong password.";
            return true;
        }

        public bool Register(string username, string password, UserRole role)
        {
            if (_userRepository.GetAll().Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                return false;

            if (!ValidatePasswordStrength(password, out _)) return false;

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            string hashBase64 = HashPassword(password, salt);

            var user = new User
            {
                Username = username,
                PasswordHash = hashBase64,
                Salt = Convert.ToBase64String(salt),
                Role = role,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return _userRepository.Add(user) > 0;
        }

        public bool EnsureUser(string username, string password, UserRole role)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            string hashBase64 = HashPassword(password, salt);

            if (user == null)
            {
                user = new User
                {
                    Username = username,
                    PasswordHash = hashBase64,
                    Salt = Convert.ToBase64String(salt),
                    Role = role,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                return _userRepository.Add(user) > 0;
            }
            else
            {
                user.PasswordHash = hashBase64;
                user.Salt = Convert.ToBase64String(salt);
                user.Role = role; // Ensure role is correct too
                user.FailedLoginAttempts = 0;
                user.LockoutEnd = null;
                user.UpdatedAt = DateTime.Now;
                _userRepository.Update(user);
                return true;
            }
        }

        private string HashPassword(string password, byte[] salt)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
            return Convert.ToBase64String(hash);
        }

        public bool Login(string username, string password, out string errorMessage)
        {
            errorMessage = "";
            var users = _userRepository.GetAll();
            var user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                errorMessage = "Invalid username or password.";
                return false;
            }

            // Check Lockout
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.Now)
            {
                var remaining = user.LockoutEnd.Value - DateTime.Now;
                errorMessage = $"Account is locked. Try again in {Math.Ceiling(remaining.TotalMinutes)} minutes.";
                return false;
            }

            byte[] salt = Convert.FromBase64String(user.Salt);
            byte[] storedHash = Convert.FromBase64String(user.PasswordHash);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);

            bool match = CryptographicOperations.FixedTimeEquals(hash, storedHash);

            if (match)
            {
                user.FailedLoginAttempts = 0;
                user.LockoutEnd = null;
                user.LastLogin = DateTime.Now;
                _userRepository.Update(user);

                CurrentUser = user;
                return true;
            }
            else
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= MaxFailedAttempts)
                {
                    user.LockoutEnd = DateTime.Now.AddMinutes(LockoutMinutes);
                    errorMessage = $"Too many failed attempts. Account locked for {LockoutMinutes} minutes.";
                }
                else
                {
                    errorMessage = $"Invalid username or password. {MaxFailedAttempts - user.FailedLoginAttempts} attempts remaining.";
                }
                _userRepository.Update(user);
                return false;
            }
        }


        public void ResetAllFailedAttempts()
        {
            var users = _userRepository.GetAll();
            foreach (var user in users)
            {
                if (user.FailedLoginAttempts > 0 || user.LockoutEnd.HasValue)
                {
                    user.FailedLoginAttempts = 0;
                    user.LockoutEnd = null;
                    _userRepository.Update(user);
                }
            }
        }

        public void Logout() => CurrentUser = null;
    }
}
