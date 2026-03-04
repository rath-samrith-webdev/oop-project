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
        private readonly IDatabaseService _dbService;

        public string FeatureName => "Home";
        public Form GetForm() => this;

        public HomeWelcome(IRepository<Customer> customerRepo, IRepository<LoanModel> loanRepo, IRepository<AuditLog> auditRepo)
        {
            _customerRepo = customerRepo;
            _loanRepo = loanRepo;
            _auditRepo = auditRepo;
            _dbService = ServiceContainer.GetService<IDatabaseService>();
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

            // Hero Section - Add FIRST to ensure it's at the top of the Dock stack
            Panel heroPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 200, // Slightly more height for better breathing room
                BackColor = Color.FromArgb(15, UITheme.AccentColor),
                Margin = new Padding(0, 0, 0, 20),
                Padding = new Padding(40, 45, 40, 40)
            };
            gridContainer.Controls.Add(heroPanel);

            // Labels for Hero Section
            Label welcomeLabel = new Label
            {
                Text = "Welcome to Nexus Dashboard",
                AutoSize = true,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent
            };
            UITheme.StyleLabel(welcomeLabel, UITheme.LabelLevel.Header);
            heroPanel.Controls.Add(welcomeLabel);

            Label subLabel = new Label
            {
                Text = $"System Overview • {DateTime.Now:MMMM dd, yyyy}",
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 15, 0, 0), // Added gap between texts
                ForeColor = UITheme.TextSecondary,
                BackColor = Color.Transparent
            };
            UITheme.StyleLabel(subLabel, UITheme.LabelLevel.SubHeader);
            heroPanel.Controls.Add(subLabel);

            // Bento Grid Implementation using TableLayoutPanel for responsiveness
            TableLayoutPanel bentoGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 3,
                RowCount = 2,
                Height = 620, // Sum of row heights
                Margin = new Padding(0, 20, 0, 0) // Gap between hero and grid
            };
            bentoGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            bentoGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            bentoGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            bentoGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            bentoGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            gridContainer.Controls.Add(bentoGrid);

            // Ensure Z-order puts hero panel at the top (outermost dock)
            heroPanel.SendToBack();
            bentoGrid.BringToFront();

            var customersCount = _customerRepo.GetAll().Count();
            var loansCount = _loanRepo.GetAll().Count();
            var recentAudits = _auditRepo.GetAll().Take(5).ToList();

            // Row 1: Key Stats
            AddBentoCard(bentoGrid, "Total Customers", $"{customersCount} Active Records", "👥", 0, 0, UITheme.AccentColor);
            AddBentoCard(bentoGrid, "Loan Portfolio", $"{loansCount} Managed Loans", "💼", 0, 1, UITheme.SecondaryAccent);
            AddBentoCard(bentoGrid, "System Status", "All Services Operational", "⚡", 0, 2, UITheme.SuccessColor);

            // Row 2: Recent Activity (Spans 2 columns)
            AddActivityCard(bentoGrid, "Recent Activity", recentAudits, 1, 0, 2);

            // Row 2 Sidebar: Leaderboard
            var topScores = _dbService.GetTopScores(3);
            AddLeaderboardCard(bentoGrid, "Top Players", topScores, 1, 2);
        }

        private void AddActivityCard(TableLayoutPanel parent, string title, List<AuditLog> logs, int row, int col, int colSpan = 1)
        {
            Panel card = new Panel {
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                BackColor = Color.Transparent,
                Padding = new Padding(24)
            };
            parent.Controls.Add(card, col, row);
            if (colSpan > 1) parent.SetColumnSpan(card, colSpan);

            card.Paint += (s, e) => {
                var rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                UITheme.DrawModernCard(e.Graphics, rect, false);
                using (var pen = new Pen(UITheme.AccentColor, 2))
                    e.Graphics.DrawLine(pen, 20, 0, card.Width - 20, 0);
            };

            Label titleLabel = new Label {
                Text = title,
                Dock = DockStyle.Top,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 25) // Added clear bottom margin
            };
            UITheme.StyleLabel(titleLabel, UITheme.LabelLevel.SubHeader);
            card.Controls.Add(titleLabel);

            Panel logsContainer = new Panel {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 10, 0, 0),
                AutoScroll = true
            };
            card.Controls.Add(logsContainer);

            // Add logs using BringToFront to ensure newest (first in list) stays at the top
            foreach (var log in logs)
            {
                Label logLabel = new Label {
                    Text = $"• {log.Action} {log.EntityName}: {log.Timestamp:HH:mm}",
                    Dock = DockStyle.Top,
                    Height = 35,
                    ForeColor = UITheme.TextSecondary,
                    Font = UITheme.BodyFont,
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoEllipsis = true,
                    Padding = new Padding(5, 0, 0, 0)
                };
                logsContainer.Controls.Add(logLabel);
                logLabel.BringToFront(); // Moves this to index 0, pushing previous ones to higher indices (top)
            }

            if (logs.Count == 0)
            {
                Label emptyLabel = new Label {
                    Text = "No recent activity recorded.",
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    ForeColor = UITheme.TextMuted,
                    Margin = new Padding(0, 10, 0, 0)
                };
                logsContainer.Controls.Add(emptyLabel);
            }
        }

        private void AddBentoCard(TableLayoutPanel parent, string title, string description, string icon, int row, int col, Color accentColor)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Padding = new Padding(24)
            };
            parent.Controls.Add(card, col, row);

            bool isHovered = false;

            card.MouseEnter += (s, e) => { isHovered = true; card.Invalidate(); };
            card.MouseLeave += (s, e) => { isHovered = false; card.Invalidate(); };

            card.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                UITheme.DrawModernCard(e.Graphics, rect, isHovered);

                using (var pen = new Pen(accentColor, 2))
                {
                    e.Graphics.DrawLine(pen, 20, 0, card.Width - 20, 0);
                }
            };

            Label descLabel = new Label
            {
                Text = description,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoEllipsis = true,
                ForeColor = UITheme.TextSecondary,
                TextAlign = ContentAlignment.TopLeft,
                Padding = new Padding(0, 5, 0, 0)
            };
            UITheme.StyleLabel(descLabel, UITheme.LabelLevel.Body);
            card.Controls.Add(descLabel);

            Label titleLabel = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                AutoSize = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 5, 0, 5)
            };
            UITheme.StyleLabel(titleLabel, UITheme.LabelLevel.SubHeader);
            card.Controls.Add(titleLabel);

            Label iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 32),
                Dock = DockStyle.Top,
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = accentColor,
                Margin = new Padding(0, 0, 0, 10)
            };
            card.Controls.Add(iconLabel);

            card.Click += (s, e) => {
                MessageBox.Show($"Opening {title}...", "Nexus Dashboard");
            };

            UITheme.AnimateControlEntrance(card, 100);
        }

        private void AddLeaderboardCard(TableLayoutPanel parent, string title, List<(string Player, string Game, int Score, DateTime Date)> topScores, int row, int col)
        {
            Panel card = new Panel {
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                BackColor = Color.Transparent,
                Padding = new Padding(24)
            };
            parent.Controls.Add(card, col, row);

            card.Paint += (s, e) => {
                var rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                UITheme.DrawModernCard(e.Graphics, rect, false);
                using (var pen = new Pen(UITheme.WarningColor, 2))
                    e.Graphics.DrawLine(pen, 20, 0, card.Width - 20, 0);
            };

            Label titleLabel = new Label {
                Text = title,
                Dock = DockStyle.Top,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 25) // Added clear bottom margin
            };
            UITheme.StyleLabel(titleLabel, UITheme.LabelLevel.SubHeader);
            card.Controls.Add(titleLabel);

            Panel scoresContainer = new Panel {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 10, 0, 0),
                AutoScroll = true
            };
            card.Controls.Add(scoresContainer);

            Color[] rankColors = { Color.FromArgb(255, 215, 0), Color.FromArgb(192, 192, 192), Color.FromArgb(205, 127, 50) };

            for (int i = 0; i < Math.Min(topScores.Count, 3); i++)
            {
                var score = topScores[i];
                if (string.IsNullOrWhiteSpace(score.Player)) continue;

                Panel item = new Panel {
                    Dock = DockStyle.Top,
                    Height = 50, // Back to 50 for better breathing room
                    Padding = new Padding(0, 0, 0, 10)
                };
                scoresContainer.Controls.Add(item);
                item.BringToFront(); // Ensures Rank 1 stays at the top

                // Order of adding for docking: Right, then Left, then Fill
                // We add Fill LAST and BringToFront to ensure it docks correctly in the remaining space
                Label nameLabel = new Label {
                    Text = score.Player,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.White,
                    Font = UITheme.BodyFont,
                    Padding = new Padding(15, 0, 0, 0), // Increased padding for clarity
                    AutoEllipsis = true
                };
                item.Controls.Add(nameLabel);

                Label rankLabel = new Label {
                    Text = (i + 1).ToString(),
                    Dock = DockStyle.Left,
                    Width = 40,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = rankColors[i]
                };
                item.Controls.Add(rankLabel);

                Label scoreLabel = new Label {
                    Text = score.Score.ToString(),
                    Dock = DockStyle.Right,
                    Width = 70,
                    TextAlign = ContentAlignment.MiddleRight,
                    ForeColor = rankColors[i],
                    Font = UITheme.HeaderFont,
                    Padding = new Padding(0, 0, 10, 0)
                };
                item.Controls.Add(scoreLabel);

                nameLabel.BringToFront(); // Final Z-order: nameLabel(0), scoreLabel(1), rankLabel(2)
            }

            if (topScores.Count == 0)
            {
                Label emptyLabel = new Label {
                    Text = "No records yet.",
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    ForeColor = UITheme.TextMuted,
                    Margin = new Padding(0, 10, 0, 0)
                };
                scoresContainer.Controls.Add(emptyLabel);
            }
        }
    }
}
