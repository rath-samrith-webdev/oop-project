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
            _dbService = ServiceContainer.GetService<IDatabaseService>();
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
                Height = 60,
                Padding = new Padding(0, 0, 0, 10),
                TextAlign = ContentAlignment.BottomLeft
            };
            UITheme.StyleLabel(titleLabel, UITheme.LabelLevel.Title);
            this.Controls.Add(titleLabel);

            dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            UITheme.StyleDataGrid(dataGridView);

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
