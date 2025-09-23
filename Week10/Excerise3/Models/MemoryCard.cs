// Models/MemoryCard.cs
namespace Exercise3.Models 
{
    public class MemoryCard
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = "";  
        public bool IsFlipped { get; set; }
        public bool IsMatched { get; set; }
    }
}
