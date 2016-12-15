using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Provides functions related to SolidDna plug-ins
    /// </summary>
    public static class PlugInIntegration
    {
        /// <summary>
        /// The location of the SolidDna dll file and any plug-in's
        /// </summary>
        public static string PlugInFolder { get { return Path.GetDirectoryName(typeof(PlugInIntegration).Assembly.CodeBase.Replace(@"file:\", "").Replace(@"file:///", "")); } }

        public static List<ISolidPlugIn> SolidDnaPlugIns()
        {
            // Create new empry list
            var assemblies = new List<ISolidPlugIn>();

            // Get the location of the SolidDna dll

            // Find all dll's in the same directory
            foreach (var path in Directory.GetFiles(PlugInFolder, "*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    // Load the assembly
                    var ass = Assembly.LoadFile(path);

                    // If we didn't succeed, ignore
                    if (ass == null)
                        continue;

                    var type = typeof(ISolidPlugIn);

                    // Now look through all types and see if any are of ISolidDnaPlugIn
                    ass.GetTypes().Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList().ForEach(p =>
                    {
                        // Create SolidDna plugin class instance
                        var inter = Activator.CreateInstance(p) as ISolidPlugIn;
                        assemblies.Add(inter);
                    });
                }
                catch
                {

                }
            }

            return assemblies;
        }
    }
}
