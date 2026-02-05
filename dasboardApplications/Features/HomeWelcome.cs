using System;
using System.Drawing;
using System.Windows.Forms;
using dasboardApplications.Core;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Features
{
    public class HomeWelcome : BaseFeatureForm, IFeature
    {
        public string FeatureName => "Home";
        public Form GetForm() => this;

        public HomeWelcome()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            this.BackColor = UITheme.SecondaryBackground;
            this.Padding = new Padding(40);

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true
            };
            this.Controls.Add(layout);

            var welcomeLabel = new Label
            {
                Text = "Welcome to Nexus",
                Font = new Font("Segoe UI", 28, FontStyle.Bold), // Reduced from 32
                ForeColor = UITheme.TextPrimary,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };
            layout.Controls.Add(welcomeLabel);

            var subLabel = new Label
            {
                Text = "Your premium suite for high-performance OOP applications.",
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                ForeColor = UITheme.TextSecondary,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 40)
            };
            layout.Controls.Add(subLabel);

            // Quick Stats or Features List
            AddFeatureCard(layout, "Car Racing", "Experience high-speed physics and score tracking.", "🏎️");
            AddFeatureCard(layout, "Tic Tac Toe", "Classic game logic implemented with OOP patterns.", "🎮");
            AddFeatureCard(layout, "Loan Manager", "Advanced financial calculations and amortization.", "💰");
            AddFeatureCard(layout, "Score Board", "Real-time global leaderboard data.", "🏆");
        }

        private void AddFeatureCard(Control parent, string title, string description, string icon)
        {
            var card = new Panel
            {
                Width = parent.Width - 100, // Dynamic width based on parent
                Height = 110,
                BackColor = Color.FromArgb(30, 30, 35),
                Margin = new Padding(0, 0, 0, 15),
                Padding = new Padding(20)
            };
            parent.Controls.Add(card);

            // Resize card when parent resizes
            parent.Resize += (s, e) => { card.Width = parent.Width - 100; };

            var iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 24),
                Location = new Point(20, 25),
                AutoSize = true
            };
            card.Controls.Add(iconLabel);

            var titleLabel = new Label
            {
                Text = title,
                Font = UITheme.TitleFont,
                ForeColor = UITheme.AccentColor,
                Location = new Point(80, 20),
                AutoSize = true
            };
            card.Controls.Add(titleLabel);

            var descLabel = new Label
            {
                Text = description,
                Font = UITheme.BodyFont,
                ForeColor = UITheme.TextSecondary,
                Location = new Point(80, 50),
                AutoSize = true
            };
            card.Controls.Add(descLabel);
        }
    }
}
