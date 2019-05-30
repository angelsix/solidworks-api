namespace AngelSix.SolidDna
{
    /// <summary>
    /// Settings and objects related to the current SolidDNA environment
    /// </summary>
    public static class SolidDnaEnvironment
    {
        #region Public Properties

        /// <summary>
        /// If true, any uncaught exceptions that get thrown will get caught,
        /// logged to the <see cref="ILogFactory"/> then swallowed.
        /// 
        /// WARNING: If turning this on, be aware you may get null/default values
        /// being returned from function calls if they throw errors
        /// so be vigilant on null checking if so
        /// </summary>
        public static bool LogAndIgnoreUncaughtExceptions { get; set; } = false;

        #endregion
    }
}
