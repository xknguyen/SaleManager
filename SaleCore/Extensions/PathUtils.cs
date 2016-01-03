using System;
using System.IO;

namespace SaleCore.Extensions
{
    /// <summary>
    /// Utility methods for working with resource paths
    /// </summary>
    public static class PathUtils
    {
        /// <summary>
        /// Makes a filename safe for use within a URL
        /// </summary>
        public static string MakeFileNameSafeForUrls(string fileName)
        {
            //Ensure.Argument.NotNullOrEmpty(fileName, "fileName");
            var extension = Path.GetExtension(fileName);
            var safeFileName = Path.GetFileNameWithoutExtension(fileName).ToString();
            return Path.Combine(Path.GetDirectoryName(fileName), safeFileName + extension);
        }

        /// <summary>
        /// Combines two URL paths
        /// </summary>
        public static string CombinePaths(string path1, string path2)
        {
            //Ensure.Argument.NotNull(path1, "path1");
           // Ensure.Argument.NotNull(path2, "path2");

            if (String.IsNullOrEmpty(path2))
                return path1;

            if (String.IsNullOrEmpty(path1))
                return path2;

            if (path2.StartsWith("http://") || path2.StartsWith("https://"))
                return path2;

            var ch = path1[path1.Length - 1];

            if (ch != '/')
                return (path1.TrimEnd('/') + '/' + path2.TrimStart('/'));

            return (path1 + path2);
        }
    }
}
