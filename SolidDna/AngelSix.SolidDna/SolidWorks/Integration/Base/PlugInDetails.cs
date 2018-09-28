namespace AngelSix.SolidDna
{
    /// <summary>
    /// Contains details about a SolidDna plug-in to be loaded
    /// </summary>
    public class PlugInDetails
    {
        /// <summary>
        /// The absolute path to the plug-in dll
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// The fully qualified name of the assembly, such as SolidDna.MyAssembly, ...
        /// </summary>
        public string AssemblyFullName { get; set; }

        /// <summary>
        /// The type that implements the <see cref="SolidPlugIn"/> in this assembly
        /// </summary>
        public string TypeFullName { get; set; }
    }
}
