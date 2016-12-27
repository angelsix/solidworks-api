namespace AngelSix.SolidDna
{
    /// <summary>
    /// The level of priority of a specific log message
    /// </summary>
    public enum LogFactoryLevel
    {
        /// <summary>
        /// For debugging. The least important level
        /// </summary>
        Debug = 1,
        /// <summary>
        /// Non-critical information
        /// </summary>
        Verbose = 2,
        /// <summary>
        /// Standard information
        /// </summary>
        Information = 3,
        /// <summary>
        /// A warning
        /// </summary>
        Warning = 4,
        /// <summary>
        /// An error
        /// </summary>
        Error = 5,
        /// <summary>
        /// A serious error
        /// </summary>
        Critical = 6,
    }
}
