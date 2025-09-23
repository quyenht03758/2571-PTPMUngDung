// Images/FolderImageProvider.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exercise3.Images
{
    public class FolderImageProvider : IImageProvider
    {
        private readonly string _root;
        private static readonly string[] _exts = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };

        public FolderImageProvider(string rootFolder)
        {
            _root = rootFolder ?? throw new ArgumentNullException(nameof(rootFolder));
        }

        public List<string> GetAllImageFiles()
        {
            if (!Directory.Exists(_root)) return new List<string>();

            return Directory.EnumerateFiles(_root, "*.*", SearchOption.TopDirectoryOnly)
                            .Where(p => !_IsBack(p) && _exts.Contains(Path.GetExtension(p).ToLowerInvariant()))
                            .OrderBy(p => p)
                            .ToList();

            bool _IsBack(string p) => string.Equals(Path.GetFileName(p), "back.png", StringComparison.OrdinalIgnoreCase);
        }

        public string GetBackImagePath()
        {
            var path = Path.Combine(_root, "back.png");
            return path;
        }
    }
}
