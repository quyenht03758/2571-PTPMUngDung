using Exercise3.Models;

namespace Exercise3.Data
{
    public interface ISettingsRepository
    {
        GameSettings Load();
        void Save(GameSettings settings);
    }
}
