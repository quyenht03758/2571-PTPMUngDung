using System.Collections.Generic;
using Exercise3.Models;

namespace Exercise3.Controllers
{
    public interface IGameController
    {
        GameSettings Settings { get; }
        int Score { get; }
        int Level { get; }
        int ElapsedSec { get; }

        void ApplySettings(GameSettings s);
        List<MemoryCard> StartNew();
        bool TryMatch(MemoryCard a, MemoryCard b);
        void Tick();
        void SaveScore(string player);
    }
}
