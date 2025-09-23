// Images/IImageProvider.cs
using System.Collections.Generic;

namespace Exercise3.Images
{
    public interface IImageProvider
    {
        /// <summary>
        /// Returns absolute file paths of available face images (excluding the back face).
        /// </summary>
        List<string> GetAllImageFiles();

        /// <summary>
        /// Returns absolute path to the back-face image.
        /// </summary>
        string GetBackImagePath();
    }
}
