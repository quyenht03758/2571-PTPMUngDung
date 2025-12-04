using Exercise3.Models;

namespace Exercise3.Controllers
{
    public interface ISettingsController
    {
        GameSettings GetCurrent();
        GameSettings SelectPreset(int rows, int cols);
        void SetFlip(int ms);
        void SetMismatchDelay(int ms);
    }
}
