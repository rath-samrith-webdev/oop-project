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
            this.BackColor = UITheme.PrimaryBackground;
            this.Padding = new Padding(40);

            // Container for Bento Grid
            Panel gridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            this.Controls.Add(gridContainer);

            Label welcomeLabel = new Label
            {
                Text = "Welcome to Nexus",
                AutoSize = true,
                Location = new Point(0, 0)
            };
            UITheme.StyleLabel(welcomeLabel, UITheme.LabelLevel.Header);
            gridContainer.Controls.Add(welcomeLabel);

            Label subLabel = new Label
            {
                Text = "The premium ecosystem for dynamic OOP applications.",
                AutoSize = true,
                Location = new Point(0, 80)
            };
            UITheme.StyleLabel(subLabel, UITheme.LabelLevel.SubHeader);
            gridContainer.Controls.Add(subLabel);

            // Bento Grid Implementation
            int startY = 160;

            // Large Card (Loan Manager)
            AddBentoCard(gridContainer, "Loan Manager", "Financial suite for high-precision calculations.", "💰",
                new Rectangle(0, startY, 600, 280), isLarge: true);

            // Medium Card (Tic Tac Toe)
            AddBentoCard(gridContainer, "Tic Tac Toe", "Dynamic grid logic and AI.", "🎮",
                new Rectangle(624, startY, 350, 280));

            // Medium Card (Car Racing)
            AddBentoCard(gridContainer, "Car Racing", "Physics-based racing engine.", "🏎️",
                new Rectangle(0, startY + 304, 350, 250));

            // Small Card (Score Board)
            AddBentoCard(gridContainer, "Score Board", "Live global rankings.", "🏆",
                new Rectangle(374, startY + 304, 600, 250));
        }

        private void AddBentoCard(Control parent, string title, string description, string icon, Rectangle bounds, bool isLarge = false)
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
                UITheme.DrawModernCard(e.Graphics, new Rectangle(0, 0, card.Width - 1, card.Height - 1), isHovered);
            };

            Label iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", isLarge ? 40 : 28),
                Location = new Point(24, 24),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            card.Controls.Add(iconLabel);

            Label titleLabel = new Label
            {
                Text = title,
                Location = new Point(24, isLarge ? 100 : 80),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            UITheme.StyleLabel(titleLabel, isLarge ? UITheme.LabelLevel.Header : UITheme.LabelLevel.SubHeader);
            card.Controls.Add(titleLabel);

            Label descLabel = new Label
            {
                Text = description,
                Location = new Point(24, isLarge ? 150 : 120),
                Size = new Size(card.Width - 48, 80),
                BackColor = Color.Transparent,
                AutoEllipsis = true
            };
            UITheme.StyleLabel(descLabel, UITheme.LabelLevel.Body);
            card.Controls.Add(descLabel);

            card.Click += (s, e) => {
                MessageBox.Show($"Opening {title}...", "Modern Navigation");
            };
        }
    }
}
