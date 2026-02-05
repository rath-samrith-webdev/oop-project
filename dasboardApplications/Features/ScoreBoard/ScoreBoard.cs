using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using dasboardApplications.Core;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Features.ScoreBoard
{
    public class ScoreBoard : BaseFeatureForm, IFeature
    {
        public string FeatureName => "Score Board";
        public Form GetForm() => this;

        private DataGridView dataGridView = null!;
        private readonly IDatabaseService _dbService;

        public ScoreBoard()
        {
            _dbService = ServiceContainer.Get<IDatabaseService>();
            SetupUI();
            LoadScores();
        }

        private void SetupUI()
        {
            this.Text = "High Scores";
            this.BackColor = UITheme.SecondaryBackground;
            this.Padding = new Padding(10);

            var titleLabel = new Label
            {
                Text = "GLOBAL LEADERBOARD",
                Dock = DockStyle.Top,
                Height = 40,
                Font = UITheme.TitleFont,
                ForeColor = UITheme.AccentColor,
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(titleLabel);

            dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                BackgroundColor = UITheme.SecondaryBackground,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(40, 40, 45),
                ForeColor = UITheme.TextPrimary,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Custom styling for headers and cells
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 35);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = UITheme.TextSecondary;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = UITheme.BodyFont;
            dataGridView.ColumnHeadersHeight = 45;

            dataGridView.DefaultCellStyle.BackColor = UITheme.SecondaryBackground;
            dataGridView.DefaultCellStyle.ForeColor = UITheme.TextPrimary;
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(40, UITheme.AccentColor);
            dataGridView.DefaultCellStyle.SelectionForeColor = UITheme.TextPrimary;
            dataGridView.RowTemplate.Height = 35;

            this.Controls.Add(dataGridView);
            this.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 10 }); // Spacer
        }

        private void LoadScores()
        {
            try
            {
                var scores = _dbService.GetTopScores(50)
                    .Select(s => new {
                        Player = s.Player,
                        Game = s.Game,
                        Score = s.Score,
                        Date = s.Date.ToString("yyyy-MM-dd HH:mm")
                    })
                    .ToList();

                dataGridView.DataSource = scores;
                dataGridView.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading scores: {ex.Message}", "Database Error");
            }
        }
    }
}
