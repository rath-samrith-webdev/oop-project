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
using dasboardApplications.Features;

namespace dasboardApplications
{
    public partial class Dashboard : Form
    {
        private readonly FeatureManager _featureManager;
        private Panel sidebarPanel = null!;
        private Panel contentPanel = null!;
        private Label statusLabel = null!;
        private Button? activeButton = null;
        private Panel activeIndicator = null!;

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
            this.Text = "Nexus Dashboard";
            this.Size = new Size(1366, 850);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UITheme.ContentBackground;
            this.Font = UITheme.BodyFont;
        }

        private void RegisterFeatures()
        {
            _featureManager.RegisterFeature(() => new HomeWelcome());
            _featureManager.RegisterFeature(() => new CarRacing());
            _featureManager.RegisterFeature(() => new TicTacToe());
            _featureManager.RegisterFeature(() => new ScoreBoard());
            _featureManager.RegisterFeature(() => new LoanForm());
        }

        private void CreateModernUI()
        {
            this.Controls.Clear();

            // Root TableLayoutPanel for the entire Form
            TableLayoutPanel rootLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = UITheme.PrimaryBackground
            };
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260f)); // Sidebar Column
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f)); // Content Column
            this.Controls.Add(rootLayout);

            // Sidebar (Left - Col 0)
            sidebarPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UITheme.PrimaryBackground,
                Padding = new Padding(15, 30, 15, 30)
            };
            rootLayout.Controls.Add(sidebarPanel, 0, 0);

            // Main Content Container (Col 1)
            TableLayoutPanel contentAreaGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = UITheme.ContentBackground
            };
            contentAreaGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            contentAreaGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 80f)); // Header Row
            contentAreaGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Feature Row
            rootLayout.Controls.Add(contentAreaGrid, 1, 0);

            // App Logo/Title Area in Sidebar
            Label logoLabel = new Label
            {
                Text = "NEXUS",
                Dock = DockStyle.Top,
                Height = 60,
                Font = new Font("Segoe UI Semibold", 20, FontStyle.Bold),
                ForeColor = UITheme.AccentColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 0, 30)
            };
            sidebarPanel.Controls.Add(logoLabel);

            // Status Label (Row 0 of Content Area)
            statusLabel = new Label
            {
                Dock = DockStyle.Fill,
                Text = "DASHBOARD",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = UITheme.HeaderFont,
                BackColor = Color.Transparent,
                ForeColor = UITheme.TextPrimary,
                Padding = new Padding(40, 0, 0, 0)
            };
            contentAreaGrid.Controls.Add(statusLabel, 0, 0);

            // Active Indicator (Vertical Bar)
            activeIndicator = new Panel
            {
                BackColor = UITheme.AccentColor,
                Width = 4,
                Height = 50,
                Visible = false
            };
            sidebarPanel.Controls.Add(activeIndicator);

            // Feature Content Panel (Inside Canvas Panel)
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UITheme.SecondaryBackground,
                Padding = new Padding(30),
                AutoScroll = true
            };

            // Canvas Panel (Row 1 of Content Area)
            Panel canvasPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };
            contentAreaGrid.Controls.Add(canvasPanel, 0, 1);
            canvasPanel.Controls.Add(contentPanel);

            // Create buttons for each feature in Sidebar
            int yPos = 120; // Start below logo
            foreach (var feature in _featureManager.GetFeatures())
            {
                Button btn = CreateSidebarButton(feature, yPos);
                sidebarPanel.Controls.Add(btn);
                yPos += 65;
            }

            activeIndicator.BringToFront();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }

        private Button CreateSidebarButton(IFeature initialInstance, int y)
        {
            // We need a way to recreate this feature, so we find its type or store a way to recreate it.
            // For now, we'll store the Type in the button and recreate it on click.
            Type featureType = initialInstance.GetType();

            Button btn = new Button
            {
                Text = "      " + initialInstance.FeatureName,
                Width = 230,
                Height = 55,
                Location = new Point(15, y),
                FlatStyle = FlatStyle.Flat,
                ForeColor = UITheme.TextSecondary,
                BackColor = Color.Transparent,
                Font = UITheme.ButtonFont,
                TextAlign = ContentAlignment.MiddleLeft,
                Tag = featureType,
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, UITheme.AccentColor);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(20, UITheme.AccentColor);

            btn.MouseEnter += (s, e) => { if (btn != activeButton) btn.ForeColor = UITheme.TextPrimary; };
            btn.MouseLeave += (s, e) => { if (btn != activeButton) btn.ForeColor = UITheme.TextSecondary; };

            btn.Click += (s, e) => {
                SetActiveButton(btn);
                IFeature? newFeature = Activator.CreateInstance(featureType) as IFeature;
                if (newFeature != null) ShowFeature(newFeature);
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
            activeButton.BackColor = Color.FromArgb(40, UITheme.AccentColor);
            activeButton.ForeColor = UITheme.TextPrimary;

            activeIndicator.Location = new Point(0, btn.Location.Y);
            activeIndicator.Height = btn.Height;
            activeIndicator.Visible = true;
            activeIndicator.BringToFront();
        }

        private void ShowFeature(IFeature feature)
        {
            // Close old feature forms gracefully
            var oldControls = contentPanel.Controls.Cast<Control>().ToList();
            foreach (Control ctrl in oldControls)
            {
                if (ctrl is Form f)
                {
                    f.Close();
                    f.Dispose();
                }
                else
                {
                    ctrl.Dispose();
                }
            }
            contentPanel.Controls.Clear();

            var form = feature.GetForm();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            // Ensure child form uses our theme colors as base
            form.BackColor = UITheme.SecondaryBackground;

            contentPanel.Controls.Add(form);
            form.Show();

            if (form is BaseFeatureForm bf) bf.OnFeatureFocused();

            statusLabel.Text = feature.FeatureName.ToUpper();

            // Re-layout to respect padding
            contentPanel.PerformLayout();
        }
    }
}
