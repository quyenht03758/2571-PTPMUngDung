// Controllers/GameController.cs
using Exercise3.Data;
using Exercise3.Images;
using Exercise3.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise3.Controllers
{
    public class GameController
    {
        private readonly IImageProvider _imageProvider;
        private readonly ScoreRepository _scores;

        public GameSettings Settings { get; private set; }
        public int Score { get; private set; }
        public int Level { get; private set; } = 1;
        public int ElapsedSec { get; private set; }

        private readonly Random _rnd = new Random();
        private List<MemoryCard> _cards = new List<MemoryCard>();

        public GameController(IImageProvider imageProvider, ScoreRepository scoreRepo, GameSettings settings)
        {
            if (imageProvider == null) throw new ArgumentNullException("imageProvider");
            if (scoreRepo == null) throw new ArgumentNullException("scoreRepo");
            if (settings == null) throw new ArgumentNullException("settings");

            _imageProvider = imageProvider;
            _scores = scoreRepo;
            Settings = settings;
        }

        public void ApplySettings(GameSettings s)
        {
            if (s == null) throw new ArgumentNullException("s");
            Settings = s;
        }

        public void Tick() { ElapsedSec++; }

        public void SaveScore(string player)
        {
            _scores.Save(player, Score, Level, ElapsedSec);
        }

        /// <summary>
        /// Build a deck that adapts to the number of files available:
        /// - If we have enough unique images: pick exactly 'pairs' distinct images.
        /// - If not enough: round-robin repeat available images until we reach 'pairs'.
        /// Each chosen image produces exactly TWO cards (one pair).
        /// </summary>
        public List<MemoryCard> StartNew()
        {
            Score = 0;
            ElapsedSec = 0;

            int cells = Settings.Rows * Settings.Cols;
            if (cells % 2 != 0)
                throw new InvalidOperationException("Board size must contain an even number of cells.");

            int pairs = cells / 2;

            // Load all available image files (excluding back.png by provider)
            var files = _imageProvider.GetAllImageFiles();
            // De-dup just in case
            var unique = files.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            // If there are zero usable images, that's a hard error
            if (unique.Count == 0)
                throw new InvalidOperationException("No face images found in the 'images' folder.");

            // Shuffle the unique images first (so repetition order is random)
            Shuffle(unique);

            // Build the list of 'chosen' image paths of length 'pairs'
            var chosen = new List<string>(pairs);

            if (unique.Count >= pairs)
            {
                // Enough unique images: take exactly 'pairs' of distinct images
                chosen.AddRange(unique.GetRange(0, pairs));
            }
            else
            {
                // Not enough unique images:
                // Round-robin through the available images until we fill 'pairs'
                int idx = 0;
                while (chosen.Count < pairs)
                {
                    chosen.Add(unique[idx]);
                    idx++;
                    if (idx >= unique.Count)
                    {
                        // Reshuffle for better randomness between cycles
                        Shuffle(unique);
                        idx = 0;
                    }
                }
            }

            // Now expand each chosen image into exactly two cards (one pair)
            var deckPaths = new List<string>(cells);
            foreach (var path in chosen)
            {
                deckPaths.Add(path);
                deckPaths.Add(path);
            }

            // Final shuffle to place cards randomly on the board
            Shuffle(deckPaths);

            _cards = new List<MemoryCard>(deckPaths.Count);
            for (int i = 0; i < deckPaths.Count; i++)
            {
                _cards.Add(new MemoryCard
                {
                    Id = i,
                    ImagePath = deckPaths[i],
                    IsFlipped = false,
                    IsMatched = false
                });
            }

            return _cards;
        }

        public bool TryMatch(MemoryCard a, MemoryCard b)
        {
            bool ok = string.Equals(a.ImagePath, b.ImagePath, StringComparison.OrdinalIgnoreCase);
            if (ok)
            {
                a.IsMatched = true;
                b.IsMatched = true;
                Score += 10;
            }
            else
            {
                Score = Math.Max(0, Score - 2);
            }
            return ok;
        }

        // Fisher–Yates shuffle
        private void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = _rnd.Next(i + 1);
                if (j == i) continue;
                T tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }
        }
    }
}
