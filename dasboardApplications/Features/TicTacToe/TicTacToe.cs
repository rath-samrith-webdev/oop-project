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
        private IDatabaseService _dbService;

        public TicTacToe()
        {
            _dbService = ServiceContainer.Get<IDatabaseService>();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Tic Tac Toe - Dynamic Grid";
            this.Size = new Size(600, 700);

            FlowLayoutPanel controls = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(10) };

            controls.Controls.Add(new Label { Text = "Grid Size:", AutoSize = true, Padding = new Padding(0, 5, 0, 0) });
            sizeInput = new NumericUpDown { Value = 3, Minimum = 3, Maximum = 15, Width = 50 };
            controls.Controls.Add(sizeInput);

            controls.Controls.Add(new Label { Text = "Mode:", AutoSize = true, Padding = new Padding(10, 5, 0, 0) });
            modeComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100 };
            modeComboBox.Items.AddRange(Enum.GetNames(typeof(TicTacToeEngine.GameMode)));
            modeComboBox.SelectedIndex = 0;
            controls.Controls.Add(modeComboBox);

            startButton = new Button { Text = "Start Game", Width = 100 };
            startButton.Click += StartButton_Click;
            controls.Controls.Add(startButton);

            gridPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

            this.Controls.Add(gridPanel);
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
                        Location = new Point(c * btnSize, r * btnSize),
                        Font = new Font("Arial", btnSize / 3, FontStyle.Bold),
                        Tag = new Point(r, c)
                    };
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
            buttons[row, col].ForeColor = (player == TicTacToeEngine.PlayerType.X) ? Color.Blue : Color.Red;
        }

        private void Engine_OnGameEnded(string message)
        {
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
