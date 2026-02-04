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
            this.Size = new Size(500, 400);

            dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White
            };

            this.Controls.Add(dataGridView);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading scores: {ex.Message}", "Database Error");
            }
        }
    }
}
