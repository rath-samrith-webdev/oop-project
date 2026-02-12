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
            _dbService = ServiceContainer.GetService<IDatabaseService>();
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
                Height = 80,
                Padding = new Padding(32, 20, 32, 0),
                BackColor = Color.FromArgb(20, UITheme.AccentColor),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            var lblSize = new Label { Text = "GRID SIZE", AutoSize = true, ForeColor = UITheme.TextSecondary, Font = UITheme.SmallFont, Margin = new Padding(0, 12, 8, 0) };
            sizeInput = new NumericUpDown { Value = 3, Minimum = 3, Maximum = 10, Width = 60, Height = 32, BackColor = UITheme.SecondaryBackground, ForeColor = UITheme.TextPrimary, BorderStyle = BorderStyle.FixedSingle, Font = UITheme.BodyFont };

            var lblMode = new Label { Text = "GAME MODE", AutoSize = true, ForeColor = UITheme.TextSecondary, Font = UITheme.SmallFont, Margin = new Padding(24, 12, 8, 0) };
            modeComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 140, Height = 32, BackColor = UITheme.SecondaryBackground, ForeColor = UITheme.TextPrimary, FlatStyle = FlatStyle.Flat, Font = UITheme.BodyFont };
            modeComboBox.Items.AddRange(Enum.GetNames(typeof(TicTacToeEngine.GameMode)));
            modeComboBox.SelectedIndex = 0;

            startButton = new Button
            {
                Text = "NEW GAME",
                Width = 140,
                Height = 36,
                Margin = new Padding(32, 0, 0, 0)
            };
            UITheme.StyleButton(startButton);
            startButton.Click += StartButton_Click;

            controls.Controls.Add(lblSize);
            controls.Controls.Add(sizeInput);
            controls.Controls.Add(lblMode);
            controls.Controls.Add(modeComboBox);
            controls.Controls.Add(startButton);

            turnLabel = new Label
            {
                Text = "PLAYER X TURN",
                AutoSize = true,
                ForeColor = UITheme.AccentColor,
                Font = UITheme.TitleFont,
                Margin = new Padding(40, 10, 0, 0)
            };
            controls.Controls.Add(turnLabel);

            gridPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), BackColor = Color.Transparent };

            // In WinForms, add Top/Bottom docked controls BEFORE the Fill docked control
            this.Controls.Add(controls);
            this.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 20, BackColor = Color.Transparent }); // Spacer
            this.Controls.Add(gridPanel);
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

            // Ensure we have layout before calculating sizes
            gridPanel.Parent.Refresh();

            int padding = 20;
            int availableWidth = Math.Max(gridPanel.Width, 300) - (padding * 2);
            int availableHeight = Math.Max(gridPanel.Height, 300) - (padding * 2);
            int btnSize = Math.Min(availableWidth, availableHeight) / size - 5;

            if (btnSize < 30) btnSize = 60; // Better fallback for visibility

            int totalGridWidth = size * (btnSize + 5);
            int totalGridHeight = size * (btnSize + 5);
            int offsetX = (gridPanel.Width - totalGridWidth) / 2;
            int offsetY = (gridPanel.Height - totalGridHeight) / 2;

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    Button btn = new Button
                    {
                        Size = new Size(btnSize, btnSize),
                        Location = new Point(offsetX + c * (btnSize + 5), offsetY + r * (btnSize + 5)),
                        Font = new Font("Segoe UI", btnSize / 2.8f, FontStyle.Bold),
                        BackColor = UITheme.SecondaryBackground,
                        ForeColor = UITheme.TextPrimary,
                        FlatStyle = FlatStyle.Flat,
                        Padding = new Padding(0),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Tag = new Point(r, c),
                        Cursor = Cursors.Hand
                    };
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = UITheme.BorderColor;
                    btn.FlatAppearance.MouseOverBackColor = UITheme.HoverColor;
                    btn.FlatAppearance.MouseDownBackColor = UITheme.PressedColor;
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
            buttons[row, col].ForeColor = (player == TicTacToeEngine.PlayerType.X) ? UITheme.AccentColor : UITheme.DangerColor;
            buttons[row, col].BackColor = UITheme.SecondaryBackground;

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
