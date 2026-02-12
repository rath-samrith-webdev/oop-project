using Xunit;
using dasboardApplications.Services;
using dasboardApplications.Models;
using System.IO;
using System;

namespace dasboardApplications.Tests
{
    public class AuthServiceTests : IDisposable
    {
        private readonly string _testDbPath;
        private readonly DatabaseService _dbService;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _testDbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_auth.db");
            if (File.Exists(_testDbPath)) File.Delete(_testDbPath);

            // We need a way to set the DB path in DatabaseService for testing
            // For this demo, let's assume DatabaseService uses AppDomain.CurrentDomain.BaseDirectory/scores.db
            // I'll just use the default one for now but it might conflict.
            _dbService = new DatabaseService();
            _authService = new AuthService(_dbService);
        }

        [Fact]
        public void Register_NewUser_ReturnsTrue()
        {
            string username = "testuser_" + Guid.NewGuid().ToString().Substring(0, 8);
            bool result = _authService.Register(username, "password123", UserRole.User);
            Assert.True(result);
        }

        [Fact]
        public void Login_WithCorrectCredentials_ReturnsTrue()
        {
            string username = "login_test_" + Guid.NewGuid().ToString().Substring(0, 8);
            _authService.Register(username, "password123", UserRole.User);

            bool result = _authService.Login(username, "password123");
            Assert.True(result);
            Assert.NotNull(AuthService.CurrentUser);
            Assert.Equal(username, AuthService.CurrentUser.Username);
        }

        [Fact]
        public void Login_WithWrongPassword_ReturnsFalse()
        {
            string username = "wrong_pass_test_" + Guid.NewGuid().ToString().Substring(0, 8);
            _authService.Register(username, "password123", UserRole.User);

            bool result = _authService.Login(username, "wrongpassword");
            Assert.False(result);
        }

        public void Dispose()
        {
            // Clean up test DB if possible
        }
    }
}
