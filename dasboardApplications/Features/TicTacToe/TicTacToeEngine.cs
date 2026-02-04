using System;
using System.Collections.Generic;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Features.TicTacToe
{
    public class TicTacToeEngine : IGameEngine
    {
        public enum PlayerType { None, X, O }
        public enum GameMode { PvP, PvE, EvE }

        public event Action<int> OnScoreChanged;
        public event Action<string> OnGameOver;

        // Custom events for UI
        public event Action<int, int, PlayerType>? OnMoveMade;
        public event Action<PlayerType, bool>? OnInternalGameEnded;

        private PlayerType[,] grid;
        private int size;
        private PlayerType currentPlayer;
        private GameMode mode;
        private Random random = new Random();

        public int Size => size;
        public PlayerType CurrentPlayer => currentPlayer;

        public TicTacToeEngine(int gridSize, GameMode gameMode)
        {
            size = gridSize;
            mode = gameMode;
            grid = new PlayerType[size, size];
            currentPlayer = PlayerType.X;
        }

        public void Start()
        {
            // Reset logic
            grid = new PlayerType[size, size];
            currentPlayer = PlayerType.X;
        }

        public void Reset() => Start();

        public void Update() { } // Not needed for turn-based

        public bool MakeMove(int row, int col)
        {
            if (row < 0 || row >= size || col < 0 || col >= size || grid[row, col] != PlayerType.None)
                return false;

            grid[row, col] = currentPlayer;
            OnMoveMade?.Invoke(row, col, currentPlayer);

            if (CheckWin(row, col))
            {
                OnInternalGameEnded?.Invoke(currentPlayer, false);
                OnGameOver?.Invoke($"Player {currentPlayer} wins!");
            }
            else if (IsGridFull())
            {
                OnInternalGameEnded?.Invoke(PlayerType.None, true);
                OnGameOver?.Invoke("The game is a draw!");
            }
            else
            {
                SwitchPlayer();
                if (mode == GameMode.PvE && currentPlayer == PlayerType.O)
                {
                    MakeAIMove();
                }
            }

            return true;
        }

        public void MakeAIMove()
        {
            var emptyCells = new List<(int, int)>();
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (grid[r, c] == PlayerType.None)
                        emptyCells.Add((r, c));
                }
            }

            if (emptyCells.Count > 0)
            {
                var move = emptyCells[random.Next(emptyCells.Count)];
                MakeMove(move.Item1, move.Item2);
            }
        }

        private void SwitchPlayer()
        {
            currentPlayer = (currentPlayer == PlayerType.X) ? PlayerType.O : PlayerType.X;
        }

        private bool IsGridFull()
        {
            foreach (var cell in grid)
            {
                if (cell == PlayerType.None) return false;
            }
            return true;
        }

        private bool CheckWin(int row, int col)
        {
            PlayerType player = grid[row, col];
            // Check row
            bool win = true;
            for (int i = 0; i < size; i++) if (grid[row, i] != player) { win = false; break; }
            if (win) return true;

            // Check col
            win = true;
            for (int i = 0; i < size; i++) if (grid[i, col] != player) { win = false; break; }
            if (win) return true;

            // Check main diagonal
            if (row == col)
            {
                win = true;
                for (int i = 0; i < size; i++) if (grid[i, i] != player) { win = false; break; }
                if (win) return true;
            }

            // Check anti diagonal
            if (row + col == size - 1)
            {
                win = true;
                for (int i = 0; i < size; i++) if (grid[i, size - 1 - i] != player) { win = false; break; }
                if (win) return true;
            }

            return false;
        }
    }
}
