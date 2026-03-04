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

        // New properties for smooth movement
        public float PlayerVelocity { get; set; } = 0f;
        public float Acceleration { get; set; } = 0.8f;
        public float Friction { get; set; } = 0.92f;
        public float MaxVelocity { get; set; } = 12f;

        // Invulnerability
        public int InvulnerabilityTicks { get; private set; } = 0;
        public bool IsInvulnerable => InvulnerabilityTicks > 0;

        // Bonus Items
        public List<Rectangle> Coins { get; private set; } = new List<Rectangle>();

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
            PlayerVelocity = 0;
            InvulnerabilityTicks = 0;
            Coins.Clear();
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

            if (InvulnerabilityTicks > 0)
                InvulnerabilityTicks--;

            // Move coins
            for (int i = Coins.Count - 1; i >= 0; i--)
            {
                var coin = Coins[i];
                coin.Y += _roadSpeed;
                Coins[i] = coin;

                if (coin.Top > GameHeight)
                    Coins.RemoveAt(i);
                else if (coin.IntersectsWith(PlayerBounds))
                {
                    Coins.RemoveAt(i);
                    _score += 5;
                    OnScoreChanged?.Invoke(_score);
                }
            }

            // Spawn coins randomly
            if (_random.Next(0, 100) < 3 && Coins.Count < 3)
            {
                Coins.Add(new Rectangle(_random.Next(80, GameWidth - 80), -50, 20, 20));
            }
        }

        public bool CheckCollision()
        {
            if (IsInvulnerable) return false;

            bool hit = PlayerBounds.IntersectsWith(Enemy1Bounds) || PlayerBounds.IntersectsWith(Enemy2Bounds);
            if (hit)
            {
                InvulnerabilityTicks = 100; // ~2 seconds at 50fps
            }
            return hit;
        }

        public void TriggerGameOver(string message)
        {
            _isGameOver = true;
            OnGameOver?.Invoke(message);
        }

        public void IncrementScore()
        {
            _score++;
            OnScoreChanged?.Invoke(_score);

            if (_score > 0 && _score % 15 == 0) // Slightly harder level up
            {
                _level++;
                _roadSpeed += 1; // More gradual speed increase
                OnLevelChanged?.Invoke(_level);
            }
        }

        public int GetRandomEnemyX(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
