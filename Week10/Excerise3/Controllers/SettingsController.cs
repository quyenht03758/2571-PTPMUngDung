// Controllers/SettingsController.cs
using Exercise3.Data;
using Exercise3.Models;
using System;
using System.Drawing.Printing;

namespace Exercise3.Controllers
{
    public class SettingsController
    {
        private readonly SettingsRepository _repo;
        public SettingsController(SettingsRepository repo) => _repo = repo;

        public GameSettings GetCurrent() => _repo.Load();

        public GameSettings SelectPreset(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0) throw new ArgumentException("Rows/Cols must be positive.");
            int cells = rows * cols;
            if (cells % 2 != 0) throw new InvalidOperationException("Total cells must be even.");

            var s = _repo.Load();
            s.Rows = rows; s.Cols = cols; s.Pairs = cells / 2;
            _repo.Save(s);
            return s;
        }

        public void SetFlip(int ms)
        {
            var s = _repo.Load(); s.FlipMs = ms; _repo.Save(s);
        }

        public void SetMismatchDelay(int ms)
        {
            var s = _repo.Load(); s.MismatchDelayMs = ms; _repo.Save(s);
        }
    }
}
