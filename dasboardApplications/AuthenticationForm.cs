using System;
using System.Drawing;
using System.Windows.Forms;
using dasboardApplications.Core;
using dasboardApplications.Services;
using dasboardApplications.Models;

namespace dasboardApplications
{
    public partial class AuthenticationForm : Form
    {
        private readonly AuthService _authService;

        public AuthenticationForm()
        {
            InitializeComponent();
            _authService = dasboardApplications.Core.ServiceContainer.GetService<AuthService>();
            ApplyTheme();

            // Seed an admin user if none exists for demonstration
            SeedAdminUser();
        }

        private void SeedAdminUser()
        {
            // Ensure admin user exists with the correct password
            _authService.EnsureUser("admin", "Admin@123", UserRole.Admin);

            // Reset all failed attempts on startup as requested
            _authService.ResetAllFailedAttempts();
        }

        private void ApplyTheme()
        {
            this.BackColor = UITheme.ContentBackground;

            UITheme.StyleLabel(titleLabel, UITheme.LabelLevel.Header);
            titleLabel.Text = "Login"; // Update text to be more standard if needed, or keep "Sign in"

            // Labels
            if (userNameLabel != null) UITheme.StyleLabel(userNameLabel, UITheme.LabelLevel.Body);
            if (passwordLabel != null) UITheme.StyleLabel(passwordLabel, UITheme.LabelLevel.Body);

            // TextBoxes
            UITheme.StyleTextBox(usernameTextBox);
            UITheme.StyleTextBox(passwordTextBox);
            passwordTextBox.UseSystemPasswordChar = true;

            // Buttons
            UITheme.StyleButton(signInButton, isPrimary: true);
        }

        private void signInButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_authService.Login(username, password, out string errorMessage))
            {
                Dashboard dsh = new Dashboard();
                dsh.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(errorMessage, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
