namespace AngelSix.SolidDna
{
    /// <summary>
    /// A class providing shorthand access to commonly used values
    /// </summary>
    public static class Dna
    {
        /// <summary>
        /// The currently running instance of SolidWorks
        /// </summary>
        public static SolidWorksApplication Application { get { return AddInIntegration.SolidWorks; } }
    }
}
