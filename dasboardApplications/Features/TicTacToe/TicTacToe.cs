using System;
using System.Drawing;
using System.Windows.Forms;
using dasboardApplications.Core;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Features.TicTacToe
{
    public class TicTacToe : BaseFeatureForm, IFeature
    {
        public string FeatureName => "Tic Tac Toe";
        public Form GetForm() => this;

        private TicTacToeEngine? engine;
        private Button[,] buttons = new Button[0, 0];
        private Panel gridPanel;
        private ComboBox modeComboBox;
        private NumericUpDown sizeInput;
        private Button startButton;
        private Label turnLabel;
        private IDatabaseService _dbService;

        public TicTacToe()
        {
            _dbService = ServiceContainer.Get<IDatabaseService>();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Tic Tac Toe - Dynamic Grid";
            this.BackColor = UITheme.SecondaryBackground;
            this.Padding = new Padding(20);

            FlowLayoutPanel controls = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(30, 30, 35)
            };

            var lblSize = new Label { Text = "Grid Size:", AutoSize = true, ForeColor = UITheme.TextSecondary, Margin = new Padding(0, 8, 0, 0) };
            sizeInput = new NumericUpDown { Value = 3, Minimum = 3, Maximum = 10, Width = 60, BackColor = Color.FromArgb(40, 40, 45), ForeColor = UITheme.TextPrimary, BorderStyle = BorderStyle.FixedSingle };

            var lblMode = new Label { Text = "Game Mode:", AutoSize = true, ForeColor = UITheme.TextSecondary, Margin = new Padding(20, 8, 0, 0) };
            modeComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 120, BackColor = Color.FromArgb(40, 40, 45), ForeColor = UITheme.TextPrimary, FlatStyle = FlatStyle.Flat };
            modeComboBox.Items.AddRange(Enum.GetNames(typeof(TicTacToeEngine.GameMode)));
            modeComboBox.SelectedIndex = 0;

            startButton = new Button
            {
                Text = "START NEW GAME",
                Width = 150,
                Height = 35,
                BackColor = UITheme.AccentColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = UITheme.ButtonFont,
                Margin = new Padding(30, 0, 0, 0)
            };
            startButton.FlatAppearance.BorderSize = 0;
            startButton.Click += StartButton_Click;

            controls.Controls.Add(lblSize);
            controls.Controls.Add(sizeInput);
            controls.Controls.Add(lblMode);
            controls.Controls.Add(modeComboBox);
            controls.Controls.Add(startButton);

            turnLabel = new Label
            {
                Text = "Current Turn: X",
                AutoSize = true,
                ForeColor = UITheme.AccentColor,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Margin = new Padding(30, 8, 0, 0)
            };
            controls.Controls.Add(turnLabel);

            gridPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), BackColor = Color.Transparent };

            this.Controls.Add(gridPanel);
            this.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 20 }); // Spacer
            this.Controls.Add(controls);
        }

        private void StartButton_Click(object? sender, EventArgs e)
        {
            int size = (int)sizeInput.Value;
            TicTacToeEngine.GameMode mode = (TicTacToeEngine.GameMode)Enum.Parse(typeof(TicTacToeEngine.GameMode), modeComboBox.Text);

            engine = new TicTacToeEngine(size, mode);
            engine.OnMoveMade += Engine_OnMoveMade;
            engine.OnGameOver += msg => Engine_OnGameEnded(msg);
            engine.OnInternalGameEnded += (winner, isDraw) => {
                if (!isDraw) SaveWinnerScore(winner);
            };

            CreateGrid(size);
            gridPanel.Enabled = true;
        }

        private void CreateGrid(int size)
        {
            gridPanel.Controls.Clear();
            buttons = new Button[size, size];

            int btnSize = Math.Min(gridPanel.Width, gridPanel.Height) / size - 2;

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    Button btn = new Button
                    {
                        Size = new Size(btnSize, btnSize),
                        Location = new Point(c * (btnSize + 5), r * (btnSize + 5)),
                        Font = new Font("Segoe UI", btnSize / 3, FontStyle.Bold),
                        BackColor = Color.FromArgb(40, 40, 45),
                        ForeColor = UITheme.TextPrimary,
                        FlatStyle = FlatStyle.Flat,
                        Tag = new Point(r, c)
                    };
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Click += GridButton_Click;
                    buttons[r, c] = btn;
                    gridPanel.Controls.Add(btn);
                }
            }
        }

        private void GridButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Point p && engine != null)
            {
                engine.MakeMove(p.X, p.Y);
            }
        }

        private void Engine_OnMoveMade(int row, int col, TicTacToeEngine.PlayerType player)
        {
            buttons[row, col].Text = player.ToString();
            buttons[row, col].Enabled = false;
            buttons[row, col].ForeColor = (player == TicTacToeEngine.PlayerType.X) ? Color.FromArgb(0, 122, 255) : Color.FromArgb(255, 59, 48);
            buttons[row, col].BackColor = Color.FromArgb(45, 45, 50);

            if (engine != null)
            {
                turnLabel.Text = $"Current Turn: {engine.CurrentPlayer}";
                turnLabel.ForeColor = (engine.CurrentPlayer == TicTacToeEngine.PlayerType.X) ? Color.FromArgb(0, 122, 255) : Color.FromArgb(255, 59, 48);
            }
        }

        private void Engine_OnGameEnded(string message)
        {
            if (engine != null && engine.WinningLine != null)
            {
                foreach (var (r, c) in engine.WinningLine)
                {
                    buttons[r, c].BackColor = Color.FromArgb(0, 200, 83); // Green highlight
                    buttons[r, c].ForeColor = Color.White;
                }
            }

            MessageBox.Show(message, "Game Over");
            gridPanel.Enabled = false;
        }

        private void SaveWinnerScore(TicTacToeEngine.PlayerType winner)
        {
            using (var prompt = new PlayerNamePrompt("Enter Winner's Name"))
            {
                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    _dbService.SaveScore(prompt.PlayerName, "TicTacToe", (int)sizeInput.Value * 10);
                }
            }
        }
    }
}
