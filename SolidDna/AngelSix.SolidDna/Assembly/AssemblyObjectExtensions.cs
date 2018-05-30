using System.IO;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Extension methods for objects related to assembly information
    /// </summary>
    public static class AssemblyObjectExtensions
    {
        /// <summary>
        /// Gets the full path directory of the physical file (typically .exe or .dll)
        /// for where the callers type is located
        /// </summary>
        /// <param name="self">An instance of the calling type</param>
        /// <returns></returns>
        public static string AssemblyPath(this object self)
        {
            return Path.GetDirectoryName(self.AssemblyFilePath());
        }

        /// <summary>
        /// Gets the full path (including filename) of the physical file (typically .exe or .dll)
        /// for where the callers type is located 
        /// </summary>
        /// <param name="self">An instance of the calling type</param>
        /// <returns></returns>
        public static string AssemblyFilePath(this object self)
        {
            return self.GetType().Assembly.Location;
        }
    }
}
