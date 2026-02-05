using System;
using System.Drawing;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Features.CarRacing
{
    public class CarRacingEngine : IGameEngine
    {
        public event Action<int> OnScoreChanged;
        public event Action<string> OnGameOver;
        public event Action<int>? OnLevelChanged;

        private int _score = 0;
        private int _roadSpeed = 5;
        private int _level = 1;
        private bool _isGameOver = false;
        private Random _random = new Random();

        public Rectangle PlayerBounds { get; set; }
        public Rectangle Enemy1Bounds { get; set; }
        public Rectangle Enemy2Bounds { get; set; }
        public int GameWidth { get; set; } = 400;
        public int GameHeight { get; set; } = 600;
        public int RoadSpeed => _roadSpeed;
        public int Level => _level;

        public void Start()
        {
            _score = 0;
            _roadSpeed = 5;
            _level = 1;
            _isGameOver = false;
            OnScoreChanged?.Invoke(_score);
            OnLevelChanged?.Invoke(_level);
        }

        public void Reset()
        {
            Start();
        }

        public void Update()
        {
            if (_isGameOver) return;

            // Collision check
            if (PlayerBounds.IntersectsWith(Enemy1Bounds) || PlayerBounds.IntersectsWith(Enemy2Bounds))
            {
                _isGameOver = true;
                OnGameOver?.Invoke($"Game Over! Your Score: {_score} (Level {_level})");
            }
        }

        public void IncrementScore()
        {
            _score++;
            OnScoreChanged?.Invoke(_score);

            // Level up every 10 points
            if (_score > 0 && _score % 10 == 0)
            {
                _level++;
                _roadSpeed += 2;
                OnLevelChanged?.Invoke(_level);
            }
        }

        public int GetRandomEnemyX(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
