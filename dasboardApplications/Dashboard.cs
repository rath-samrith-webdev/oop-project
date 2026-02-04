using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using dasboardApplications.Core;
using dasboardApplications.Interfaces;
using dasboardApplications.Features.CarRacing;
using dasboardApplications.Features.TicTacToe;
using dasboardApplications.Features.ScoreBoard;
using dasboardApplications.Features.LoanManagement;

namespace dasboardApplications
{
    public partial class Dashboard : Form
    {
        private readonly FeatureManager _featureManager;
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Label statusLabel;
        private Button activeButton = null;
        private Panel activeIndicator;

        public Dashboard()
        {
            InitializeComponent();
            _featureManager = new FeatureManager();
            InitializeDashboard();
            RegisterFeatures();
            CreateModernUI();
        }

        private void InitializeDashboard()
        {
            this.Text = "Advanced OOP Dashboard";
            this.Size = new Size(1280, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UITheme.ContentBackground;
        }

        private void RegisterFeatures()
        {
            _featureManager.RegisterFeature(new CarRacing());
            _featureManager.RegisterFeature(new TicTacToe());
            _featureManager.RegisterFeature(new ScoreBoard());
            _featureManager.RegisterFeature(new LoanForm());
        }

        private void CreateModernUI()
        {
            this.Controls.Clear();

            // Status Label (Top)
            statusLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 60,
                Text = "DASHBOARD",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = UITheme.HeaderFont,
                BackColor = UITheme.HeaderBackground,
                ForeColor = UITheme.TextPrimary,
                FlatStyle = FlatStyle.Flat
            };
            this.Controls.Add(statusLabel);

            // Sidebar (Left)
            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = UITheme.PrimaryBackground,
                Padding = new Padding(0, 20, 0, 0)
            };
            this.Controls.Add(sidebarPanel);

            // Active Indicator (Vertical Bar)
            activeIndicator = new Panel
            {
                BackColor = UITheme.AccentColor,
                Width = 4,
                Height = 50,
                Visible = false
            };
            sidebarPanel.Controls.Add(activeIndicator);

            // Content Panel (Fill)
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UITheme.ContentBackground,
                Padding = new Padding(20)
            };
            this.Controls.Add(contentPanel);

            // Create buttons for each feature
            int yPos = 20;
            foreach (var feature in _featureManager.GetFeatures())
            {
                Button btn = CreateSidebarButton(feature, yPos);
                sidebarPanel.Controls.Add(btn);
                yPos += 55;
            }
        }

        private Button CreateSidebarButton(IFeature feature, int y)
        {
            Button btn = new Button
            {
                Text = "  " + feature.FeatureName,
                Width = 220,
                Height = 50,
                Location = new Point(0, y),
                FlatStyle = FlatStyle.Flat,
                ForeColor = UITheme.TextSecondary,
                BackColor = Color.Transparent,
                Font = UITheme.ButtonFont,
                TextAlign = ContentAlignment.MiddleLeft,
                Tag = feature
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = UITheme.SecondaryBackground;
            btn.FlatAppearance.MouseOverBackColor = UITheme.HoverColor;

            btn.MouseEnter += (s, e) => { if (btn != activeButton) btn.ForeColor = UITheme.TextPrimary; };
            btn.MouseLeave += (s, e) => { if (btn != activeButton) btn.ForeColor = UITheme.TextSecondary; };

            btn.Click += (s, e) => {
                SetActiveButton(btn);
                ShowFeature(feature);
            };

            return btn;
        }

        private void SetActiveButton(Button btn)
        {
            if (activeButton != null)
            {
                activeButton.BackColor = Color.Transparent;
                activeButton.ForeColor = UITheme.TextSecondary;
            }

            activeButton = btn;
            activeButton.BackColor = UITheme.SecondaryBackground;
            activeButton.ForeColor = UITheme.TextPrimary;

            activeIndicator.Location = new Point(0, btn.Location.Y);
            activeIndicator.Visible = true;
            activeIndicator.BringToFront();
        }

        private void ShowFeature(IFeature feature)
        {
            contentPanel.Controls.Clear();
            var form = feature.GetForm();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(form);
            form.Show();

            if (form is BaseFeatureForm bf) bf.OnFeatureFocused();

            statusLabel.Text = feature.FeatureName.ToUpper();
        }
    }
}
