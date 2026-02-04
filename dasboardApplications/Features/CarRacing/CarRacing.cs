using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using dasboardApplications.Core;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Features.CarRacing
{
    public class CarRacing : BaseFeatureForm, IFeature
    {
        public string FeatureName => "Car Racing";
        public Form GetForm() => this;

        private int carSpeed = 5;
        private int roadSpeed = 5;
        private bool gameOver = false;
        private CarRacingEngine _engine;
        private IDatabaseService _dbService;

        private System.Windows.Forms.Timer gameTimer;
        private PictureBox playerCar;
        private PictureBox enemyCar1;
        private PictureBox enemyCar2;
        private Label scoreLabel;

        public CarRacing()
        {
            _engine = new CarRacingEngine();
            _dbService = ServiceContainer.Get<IDatabaseService>();

            _engine.OnScoreChanged += s => { if (scoreLabel != null) scoreLabel.Text = $"Score: {s}"; };
            _engine.OnGameOver += msg => EndGame(msg);

            SetupGame();
        }

        private void SetupGame()
        {
            this.Size = new Size(400, 600);
            this.BackColor = Color.Gray;
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            scoreLabel = new Label { Text = "Score: 0", Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.White, AutoSize = true, Top = 10, Left = 10 };
            this.Controls.Add(scoreLabel);

            playerCar = new PictureBox { Size = new Size(50, 80), BackColor = Color.Transparent, Top = 450, Left = 175, SizeMode = PictureBoxSizeMode.StretchImage };
            string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
            try { playerCar.Image = Image.FromFile(Path.Combine(assetsPath, "player-car.png")); } catch { playerCar.BackColor = Color.Blue; }
            this.Controls.Add(playerCar);

            enemyCar1 = new PictureBox { Size = new Size(50, 80), BackColor = Color.Transparent, Top = -100, Left = 50, SizeMode = PictureBoxSizeMode.StretchImage };
            try { enemyCar1.Image = Image.FromFile(Path.Combine(assetsPath, "obstical.png")); } catch { enemyCar1.BackColor = Color.Red; }

            enemyCar2 = new PictureBox { Size = new Size(50, 80), BackColor = Color.Transparent, Top = -400, Left = 250, SizeMode = PictureBoxSizeMode.StretchImage };
            try { enemyCar2.Image = Image.FromFile(Path.Combine(assetsPath, "obstical.png")); } catch { enemyCar2.BackColor = Color.Green; }

            this.Controls.Add(enemyCar1);
            this.Controls.Add(enemyCar2);

            gameTimer = new System.Windows.Forms.Timer { Interval = 20 };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            this.KeyDown += CarRacing_KeyDown;
        }

        private void CarRacing_KeyDown(object? sender, KeyEventArgs e)
        {
            HandleMovement(e.KeyCode);
        }

        private void HandleMovement(Keys key)
        {
            if (gameOver) return;

            if (key == Keys.Left && playerCar.Left > 10)
                playerCar.Left -= carSpeed * 2;
            if (key == Keys.Right && playerCar.Left < this.Width - 70)
                playerCar.Left += carSpeed * 2;
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            if (gameOver) return;

            // Update bounds in engine
            _engine.PlayerBounds = playerCar.Bounds;
            _engine.Enemy1Bounds = enemyCar1.Bounds;
            _engine.Enemy2Bounds = enemyCar2.Bounds;

            // Move enemy cars
            enemyCar1.Top += roadSpeed;
            enemyCar2.Top += roadSpeed;

            // Reset enemy position if out of bounds
            if (enemyCar1.Top > this.Height)
            {
                enemyCar1.Top = -100;
                enemyCar1.Left = _engine.GetRandomEnemyX(50, 150);
                _engine.IncrementScore();
            }
            if (enemyCar2.Top > this.Height)
            {
                enemyCar2.Top = -100;
                enemyCar2.Left = _engine.GetRandomEnemyX(200, 300);
                _engine.IncrementScore();
            }

            _engine.Update();
        }

        private void EndGame(string message)
        {
            gameOver = true;
            gameTimer.Stop();
            MessageBox.Show(message, "Car Racing");

            using (var prompt = new PlayerNamePrompt("Enter Your Name"))
            {
                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    _dbService.SaveScore(prompt.PlayerName, "CarRacing", int.Parse(scoreLabel.Text.Replace("Score: ", "")));
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (gameOver) return base.ProcessCmdKey(ref msg, keyData);

            if (keyData == Keys.Left || keyData == Keys.Right)
            {
                HandleMovement(keyData);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
