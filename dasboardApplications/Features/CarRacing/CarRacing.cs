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
        private Label levelLabel;
        private Button pauseButton;
        private Button startResumeButton;
        private int backgroundOffset = 0;
        private bool isPaused = false;

        public CarRacing()
        {
            _engine = new CarRacingEngine();
            _dbService = ServiceContainer.Get<IDatabaseService>();

            _engine.OnScoreChanged += s => { if (scoreLabel != null) scoreLabel.Text = $"SCORE: {s}"; };
            _engine.OnLevelChanged += l => { if (levelLabel != null) levelLabel.Text = $"LEVEL: {l}"; };
            _engine.OnGameOver += msg => EndGame(msg);

            SetupGame();
        }

        private void SetupGame()
        {
            this.BackColor = Color.FromArgb(20, 20, 25);
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            Panel scoreContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(15, 15, 20),
                Padding = new Padding(20, 10, 20, 10)
            };
            this.Controls.Add(scoreContainer);

            scoreLabel = new Label
            {
                Text = "SCORE: 0",
                Font = UITheme.HeaderFont,
                ForeColor = UITheme.AccentColor,
                AutoSize = true,
                Dock = DockStyle.Left
            };
            scoreContainer.Controls.Add(scoreLabel);

            levelLabel = new Label
            {
                Text = "LEVEL: 1",
                Font = UITheme.HeaderFont,
                ForeColor = Color.Gold,
                AutoSize = true,
                Dock = DockStyle.Left,
                Padding = new Padding(20, 0, 0, 0)
            };
            scoreContainer.Controls.Add(levelLabel);

            Label instructions = new Label
            {
                Text = "USE ARROW KEYS TO MOVE",
                Font = UITheme.BodyFont,
                ForeColor = UITheme.TextSecondary,
                AutoSize = true,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 5, 0, 0)
            };
            instructions.Controls.Add(new Panel { Dock = DockStyle.Right, Width = 20 }); // Small spacer

            startResumeButton = new Button
            {
                Text = "START",
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 200, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Dock = DockStyle.Right,
                Margin = new Padding(10, 5, 10, 5)
            };
            startResumeButton.FlatAppearance.BorderSize = 0;
            startResumeButton.Click += (s, e) => ToggleGame();
            scoreContainer.Controls.Add(startResumeButton);

            pauseButton = new Button
            {
                Text = "PAUSE",
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(255, 152, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Dock = DockStyle.Right,
                Margin = new Padding(10, 5, 10, 5),
                Enabled = false
            };
            pauseButton.FlatAppearance.BorderSize = 0;
            pauseButton.Click += (s, e) => TogglePause();
            scoreContainer.Controls.Add(pauseButton);

            playerCar = new PictureBox { Size = new Size(50, 80), BackColor = Color.Transparent, Top = 450, Left = 175, SizeMode = PictureBoxSizeMode.StretchImage };
            string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
            try { playerCar.Image = Image.FromFile(Path.Combine(assetsPath, "player-car.png")); } catch { playerCar.BackColor = Color.Blue; }
            this.Controls.Add(playerCar);

            enemyCar1 = new PictureBox { Size = new Size(50, 80), BackColor = Color.Transparent, Top = -100, Left = 50, SizeMode = PictureBoxSizeMode.StretchImage };
            try { enemyCar1.Image = Image.FromFile(Path.Combine(assetsPath, "obstical.png")); } catch { enemyCar1.BackColor = Color.Red; }
            this.Controls.Add(enemyCar1);

            enemyCar2 = new PictureBox { Size = new Size(50, 80), BackColor = Color.Transparent, Top = -400, Left = 250, SizeMode = PictureBoxSizeMode.StretchImage };
            try { enemyCar2.Image = Image.FromFile(Path.Combine(assetsPath, "obstical.png")); } catch { enemyCar2.BackColor = Color.Green; }
            this.Controls.Add(enemyCar2);

            this.Paint += CarRacing_Paint;
            gameTimer = new System.Windows.Forms.Timer { Interval = 20 };
            gameTimer.Tick += GameTimer_Tick;

            this.KeyDown += CarRacing_KeyDown;
        }

        private void ToggleGame()
        {
            if (!gameTimer.Enabled && !isPaused)
            {
                gameTimer.Start();
                startResumeButton.Text = "STOP";
                startResumeButton.BackColor = Color.FromArgb(244, 67, 54);
                pauseButton.Enabled = true;
            }
            else
            {
                gameTimer.Stop();
                startResumeButton.Text = "START";
                startResumeButton.BackColor = Color.FromArgb(0, 200, 83);
                pauseButton.Enabled = false;
                if (isPaused)
                {
                    isPaused = false;
                    pauseButton.Text = "PAUSE";
                }
            }
        }

        private void TogglePause()
        {
            if (gameOver) return;

            isPaused = !isPaused;
            if (isPaused)
            {
                gameTimer.Stop();
                pauseButton.Text = "RESUME";
                pauseButton.BackColor = Color.FromArgb(33, 150, 243);
            }
            else
            {
                gameTimer.Start();
                pauseButton.Text = "PAUSE";
                pauseButton.BackColor = Color.FromArgb(255, 152, 0);
            }
        }

        private void RestartGame()
        {
            gameOver = false;
            isPaused = false;
            _engine.Start();

            // Reset positions
            playerCar.Left = 175;
            enemyCar1.Top = -100;
            enemyCar2.Top = -400;

            startResumeButton.Text = "STOP";
            startResumeButton.BackColor = Color.FromArgb(244, 67, 54);
            pauseButton.Enabled = true;
            pauseButton.Text = "PAUSE";
            pauseButton.BackColor = Color.FromArgb(255, 152, 0);

            gameTimer.Start();
            this.Invalidate();
        }

        private void CarRacing_KeyDown(object? sender, KeyEventArgs e)
        {
            HandleMovement(e.KeyCode);
        }

        private void HandleMovement(Keys key)
        {
            if (gameOver)
            {
                if (key == Keys.R) RestartGame();
                return;
            }

            if (key == Keys.Left && playerCar.Left > 10)
                playerCar.Left -= carSpeed * 2;
            if (key == Keys.Right && playerCar.Left < this.Width - 70)
                playerCar.Left += carSpeed * 2;
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            if (gameOver || isPaused) return;

            // Update bounds in engine
            _engine.PlayerBounds = playerCar.Bounds;
            _engine.Enemy1Bounds = enemyCar1.Bounds;
            _engine.Enemy2Bounds = enemyCar2.Bounds;

            // Move enemy cars
            enemyCar1.Top += _engine.RoadSpeed;
            enemyCar2.Top += _engine.RoadSpeed;

            // Background scrolling
            backgroundOffset += _engine.RoadSpeed;
            if (backgroundOffset > 100) backgroundOffset = 0;
            this.Invalidate();

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

        private void CarRacing_Paint(object? sender, PaintEventArgs e)
        {
            // Draw road lines
            Pen roadPen = new Pen(Color.White, 5);
            roadPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            int roadCenter = this.Width / 2;
            e.Graphics.DrawLine(roadPen, roadCenter, 0 + backgroundOffset, roadCenter, this.Height + backgroundOffset);

            // Side lines
            e.Graphics.DrawLine(new Pen(Color.White, 3), 50, 0, 50, this.Height);
            e.Graphics.DrawLine(new Pen(Color.White, 3), this.Width - 50, 0, this.Width - 50, this.Height);
        }

        private void EndGame(string message)
        {
            gameOver = true;
            gameTimer.Stop();
            MessageBox.Show(message + "\n\nPress 'R' to Restart!", "Car Racing");

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

            if (keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.R)
            {
                HandleMovement(keyData);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
