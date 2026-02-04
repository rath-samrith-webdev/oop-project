using System;

namespace dasboardApplications.Interfaces
{
    /// <summary>
    /// Contract for game logic engines.
    /// </summary>
    public interface IGameEngine
    {
        event Action<int> OnScoreChanged;
        event Action<string> OnGameOver;

        void Start();
        void Reset();
        void Update(); // For real-time games
    }
}
