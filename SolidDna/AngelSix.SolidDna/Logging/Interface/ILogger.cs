namespace AngelSix.SolidDna
{
    /// <summary>
    /// A custom Logger to plug into the <see cref="ILogFactory"/> for logging events
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log the message provided
        /// </summary>
        /// <param name="level">The level of this log message</param>
        /// <param name="message">The message to be logged</param>
        void Log(LogFactoryLevel level, string message);
    }
}
