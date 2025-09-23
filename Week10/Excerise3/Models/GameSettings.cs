// Models/GameSettings.cs
namespace Exercise3.Models   
{
    public class GameSettings
    {
        public int Rows { get; set; } = 6;
        public int Cols { get; set; } = 6;
        public int Pairs { get; set; } = 18;
        public int FlipMs { get; set; } = 120;
        public int MismatchDelayMs { get; set; } = 600;
    }
}
