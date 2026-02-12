using System;
using System.Collections.Generic;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Features.TicTacToe
{
    public class TicTacToeEngine : IGameEngine
    {
        public enum PlayerType { None, X, O }
        public enum GameMode { PvP, PvE, EvE }

        public event Action<int>? OnScoreChanged;
        public event Action<string>? OnGameOver;

        // Custom events for UI
        public event Action<int, int, PlayerType>? OnMoveMade;
        public event Action<PlayerType, bool>? OnInternalGameEnded;
        public List<(int, int)>? WinningLine { get; private set; }

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
            grid = new PlayerType[size, size];
            currentPlayer = PlayerType.X;
            WinningLine = null;
        }

        public void Reset() => Start();

        public void Update() { }

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
            if (size == 3)
            {
                var bestMove = GetBestMove();
                MakeMove(bestMove.row, bestMove.col);
            }
            else
            {
                // Heuristic for larger grids
                MakeHeuristicMove();
            }
        }

        private (int row, int col) GetBestMove()
        {
            int bestScore = int.MinValue;
            (int row, int col) move = (-1, -1);

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (grid[r, c] == PlayerType.None)
                    {
                        grid[r, c] = PlayerType.O;
                        int score = Minimax(grid, 0, false);
                        grid[r, c] = PlayerType.None;
                        if (score > bestScore)
                        {
                            bestScore = score;
                            move = (r, c);
                        }
                    }
                }
            }
            return move;
        }

        private int Minimax(PlayerType[,] board, int depth, bool isMaximizing)
        {
            PlayerType winner = GetWinner(board);
            if (winner == PlayerType.O) return 10 - depth;
            if (winner == PlayerType.X) return depth - 10;
            if (IsBoardFull(board)) return 0;

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int r = 0; r < size; r++)
                {
                    for (int c = 0; c < size; c++)
                    {
                        if (board[r, c] == PlayerType.None)
                        {
                            board[r, c] = PlayerType.O;
                            int score = Minimax(board, depth + 1, false);
                            board[r, c] = PlayerType.None;
                            bestScore = Math.Max(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int r = 0; r < size; r++)
                {
                    for (int c = 0; c < size; c++)
                    {
                        if (board[r, c] == PlayerType.None)
                        {
                            board[r, c] = PlayerType.X;
                            int score = Minimax(board, depth + 1, true);
                            board[r, c] = PlayerType.None;
                            bestScore = Math.Min(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
        }

        private void MakeHeuristicMove()
        {
            // 1. Try to win
            if (TryFindWinningMove(PlayerType.O, out var winMove)) { MakeMove(winMove.r, winMove.c); return; }
            // 2. Block player
            if (TryFindWinningMove(PlayerType.X, out var blockMove)) { MakeMove(blockMove.r, blockMove.c); return; }
            // 3. Center
            if (grid[size / 2, size / 2] == PlayerType.None) { MakeMove(size / 2, size / 2); return; }
            // 4. Random
            var emptyCells = new List<(int, int)>();
            for (int r = 0; r < size; r++)
                for (int c = 0; c < size; c++)
                    if (grid[r, c] == PlayerType.None) emptyCells.Add((r, c));

            if (emptyCells.Count > 0)
            {
                var move = emptyCells[random.Next(emptyCells.Count)];
                MakeMove(move.Item1, move.Item2);
            }
        }

        private bool TryFindWinningMove(PlayerType player, out (int r, int c) move)
        {
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (grid[r, c] == PlayerType.None)
                    {
                        grid[r, c] = player;
                        bool win = CheckWinInternal(r, c, false);
                        grid[r, c] = PlayerType.None;
                        if (win) { move = (r, c); return true; }
                    }
                }
            }
            move = (-1, -1);
            return false;
        }

        private PlayerType GetWinner(PlayerType[,] board)
        {
            // Simplified check for minimax
            for (int r = 0; r < size; r++)
                if (board[r, 0] != PlayerType.None && board[r, 0] == board[r, 1] && board[r, 0] == board[r, 2]) return board[r, 0];
            for (int c = 0; c < size; c++)
                if (board[0, c] != PlayerType.None && board[0, c] == board[1, c] && board[0, c] == board[2, c]) return board[0, c];
            if (board[0, 0] != PlayerType.None && board[0, 0] == board[1, 1] && board[0, 0] == board[2, 2]) return board[0, 0];
            if (board[0, 2] != PlayerType.None && board[0, 2] == board[1, 1] && board[0, 2] == board[2, 0]) return board[0, 2];
            return PlayerType.None;
        }

        private bool IsBoardFull(PlayerType[,] board)
        {
            foreach (var cell in board) if (cell == PlayerType.None) return false;
            return true;
        }

        private void SwitchPlayer()
        {
            currentPlayer = (currentPlayer == PlayerType.X) ? PlayerType.O : PlayerType.X;
        }

        private bool IsGridFull() => IsBoardFull(grid);

        private bool CheckWin(int row, int col) => CheckWinInternal(row, col, true);

        private bool CheckWinInternal(int row, int col, bool storeLine)
        {
            PlayerType player = grid[row, col];
            var line = new List<(int, int)>();

            // Row
            line.Clear();
            for (int i = 0; i < size; i++) if (grid[row, i] == player) line.Add((row, i));
            if (line.Count == size) { if (storeLine) WinningLine = line; return true; }

            // Col
            line.Clear();
            for (int i = 0; i < size; i++) if (grid[i, col] == player) line.Add((i, col));
            if (line.Count == size) { if (storeLine) WinningLine = line; return true; }

            // Main diag
            if (row == col)
            {
                line.Clear();
                for (int i = 0; i < size; i++) if (grid[i, i] == player) line.Add((i, i));
                if (line.Count == size) { if (storeLine) WinningLine = line; return true; }
            }

            // Anti diag
            if (row + col == size - 1)
            {
                line.Clear();
                for (int i = 0; i < size; i++) if (grid[i, size - 1 - i] == player) line.Add((i, size - 1 - i));
                if (line.Count == size) { if (storeLine) WinningLine = line; return true; }
            }

            return false;
        }
    }
}
