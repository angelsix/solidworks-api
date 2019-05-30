using System;
using System.IO;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A set of helper functions related to File data
    /// </summary>
    public static class FileHelpers
    {
        /// <summary>
        /// Gets a file or folder name from a given path.
        /// If the path does not contain any folders, the passed in path is returned. 
        /// Otherwise the file/folder name is returned
        /// </summary>
        /// <param name="path">The path to get the file/folder name from</param>
        /// <returns></returns>
        public static string GetFileFolderName(string path)
        {
            // If we have an empty string, return
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            // Normalize slashes
            var normalizedPath = path.Replace('/', '\\');

            // Get the last slash
            var lastSlash = normalizedPath.LastIndexOf('\\');

            // If we don't have any folders, return the passed in path
            if (lastSlash <= 0)
                return path;

            // Return the last entry of the paths (so the folder/file name)
            return path.Substring(lastSlash + 1);
        }

        /// <summary>
        /// Gets the codebase directory of a type in a normalized format, removing any file: prefixes
        /// </summary>
        /// <param name="type">The type to get the codebase from</param>
        /// <returns></returns>
        public static string CodeBaseNormalized(this Type type)
        {
            return Path.GetDirectoryName(type.AssemblyBaseNormalized());
        }

        /// <summary>
        /// Gets the assembly base of a type in a normalized format, removing any file: prefixes
        /// </summary>
        /// <param name="type">The type to get the assembly base from</param>
        /// <returns></returns>
        public static string AssemblyBaseNormalized(this Type type)
        {
            return type.Assembly.CodeBase.Replace(@"file:\", "").Replace(@"file:///", "");
        }
    }
}
