using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using dasboardApplications.Core;
using dasboardApplications.Interfaces;
using dasboardApplications.Models;

namespace dasboardApplications.Features
{
    public class HomeWelcome : BaseFeatureForm, IFeature
    {
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<LoanModel> _loanRepo;
        private readonly IRepository<AuditLog> _auditRepo;

        public string FeatureName => "Home";
        public Form GetForm() => this;

        public HomeWelcome(IRepository<Customer> customerRepo, IRepository<LoanModel> loanRepo, IRepository<AuditLog> auditRepo)
        {
            _customerRepo = customerRepo;
            _loanRepo = loanRepo;
            _auditRepo = auditRepo;
            SetupUI();
        }

        private void SetupUI()
        {
            this.BackColor = UITheme.PrimaryBackground;
            this.Padding = new Padding(0);

            Panel gridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(40)
            };
            this.Controls.Add(gridContainer);

            // Hero Section
            Panel heroPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 250,
                BackColor = Color.FromArgb(15, UITheme.AccentColor),
                Margin = new Padding(0, 0, 0, 40)
            };
            gridContainer.Controls.Add(heroPanel);

            Label welcomeLabel = new Label
            {
                Text = "Welcome to Nexus Dashboard",
                AutoSize = true,
                Location = new Point(40, 45),
                BackColor = Color.Transparent
            };
            UITheme.StyleLabel(welcomeLabel, UITheme.LabelLevel.Header);
            heroPanel.Controls.Add(welcomeLabel);

            Label subLabel = new Label
            {
                Text = $"System Overview • {DateTime.Now:MMMM dd, yyyy}",
                AutoSize = true,
                Location = Point.Add(welcomeLabel.Location, new Size(0, 50)),
                BackColor = Color.Transparent,
                ForeColor = UITheme.TextSecondary
            };
            UITheme.StyleLabel(subLabel, UITheme.LabelLevel.SubHeader);
            heroPanel.Controls.Add(subLabel);

            // Bento Grid Implementation
            int startY = 220;
            var customersCount = _customerRepo.GetAll().Count();
            var loansCount = _loanRepo.GetAll().Count();
            var recentAudits = _auditRepo.GetAll().Take(5).ToList();

            // Row 1: Key Stats
            AddBentoCard(gridContainer, "Total Customers", $"{customersCount} Active Records", "👥",
                new Rectangle(40, startY, 320, 200), UITheme.AccentColor);

            AddBentoCard(gridContainer, "Loan Portfolio", $"{loansCount} Managed Loans", "💼",
                new Rectangle(400, startY, 320, 200), UITheme.SecondaryAccent);

            AddBentoCard(gridContainer, "System Status", "All Services Operational", "⚡",
                new Rectangle(760, startY, 320, 200), UITheme.SuccessColor);

            // Row 2: Recent Activity
            AddActivityCard(gridContainer, "Recent Activity", recentAudits,
                new Rectangle(40, startY + 240, 680, 360));

            // Row 2 Sidebar: Quick Actions
            AddBentoCard(gridContainer, "Leaderboard", "Real-time simulation active", "🏆",
                new Rectangle(760, startY + 240, 320, 360), UITheme.WarningColor);
        }

        private void AddActivityCard(Control parent, string title, List<AuditLog> logs, Rectangle bounds)
        {
            Panel card = new Panel { Bounds = bounds, BackColor = Color.Transparent };
            parent.Controls.Add(card);

            card.Paint += (s, e) => {
                var rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                UITheme.DrawModernCard(e.Graphics, rect, false);
                using (var pen = new Pen(UITheme.AccentColor, 2))
                    e.Graphics.DrawLine(pen, 20, 0, card.Width - 20, 0);
            };

            Label titleLabel = new Label { Text = title, Location = new Point(24, 24), AutoSize = true };
            UITheme.StyleLabel(titleLabel, UITheme.LabelLevel.SubHeader);
            card.Controls.Add(titleLabel);

            int itemY = 70;
            foreach (var log in logs)
            {
                Label logLabel = new Label {
                    Text = $"• {log.Action} {log.EntityName}: {log.Timestamp:HH:mm}",
                    Location = new Point(24, itemY),
                    Size = new Size(card.Width - 48, 30),
                    ForeColor = UITheme.TextSecondary,
                    Font = UITheme.BodyFont
                };
                card.Controls.Add(logLabel);
                itemY += 40;
            }

            if (logs.Count == 0)
            {
                Label emptyLabel = new Label { Text = "No recent activity recorded.", Location = new Point(24, 70), AutoSize = true, ForeColor = UITheme.TextMuted };
                card.Controls.Add(emptyLabel);
            }
        }

        private void AddBentoCard(Control parent, string title, string description, string icon, Rectangle bounds, Color accentColor)
        {
            Panel card = new Panel
            {
                Bounds = bounds,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            parent.Controls.Add(card);

            bool isHovered = false;

            card.MouseEnter += (s, e) => { isHovered = true; card.Invalidate(); };
            card.MouseLeave += (s, e) => { isHovered = false; card.Invalidate(); };

            card.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                UITheme.DrawModernCard(e.Graphics, rect, isHovered);

                // Add a colored indicator/border at the top if hovered or always
                using (var pen = new Pen(accentColor, 2))
                {
                    e.Graphics.DrawLine(pen, 20, 0, card.Width - 20, 0);
                }
            };

            Label iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 32),
                Location = new Point(24, 24),
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = accentColor
            };
            card.Controls.Add(iconLabel);

            Label titleLabel = new Label
            {
                Text = title,
                Location = new Point(24, 90),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            UITheme.StyleLabel(titleLabel, UITheme.LabelLevel.SubHeader);
            card.Controls.Add(titleLabel);

            Label descLabel = new Label
            {
                Text = description,
                Location = new Point(24, 130),
                Size = new Size(card.Width - 48, 80),
                BackColor = Color.Transparent,
                AutoEllipsis = true,
                ForeColor = UITheme.TextSecondary
            };
            UITheme.StyleLabel(descLabel, UITheme.LabelLevel.Body);
            card.Controls.Add(descLabel);

            card.Click += (s, e) => {
                // Future: Navigate to feature
                MessageBox.Show($"Opening {title}...", "Nexus Dashboard");
            };

            UITheme.AnimateControlEntrance(card, 100);
        }
    }
}
