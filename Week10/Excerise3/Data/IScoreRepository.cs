using Exercise3.Data;

namespace Exercise3.Data
{
    public interface IScoreRepository
    {
        void Save(string player, int score, int level, int durationSec);
    }
}
