using System.Runtime.CompilerServices;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A Logger used to log messages for the application.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Log a message to the injected ILogFactory class that the application is using, if any is used
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The level of severity of this log message</param>
        /// <param name="memberName">The name of the calling method. The default is the calling method's name.</param>
        /// <param name="filePath">The file path to the calling method. The default is the callers method file path</param>
        /// <param name="lineNumber">The line number in source code of the caller method. The default is the caller methods line number</param>
        public static void Log(string message, LogFactoryLevel level = LogFactoryLevel.Information,
                                    [CallerMemberName] string memberName = "",
                                    [CallerFilePath] string filePath = "",
                                    [CallerLineNumber] int lineNumber = 0)
        {
            IoCContainer.Get<ILogFactory>().Log(message, level, $"[{FileHelpers.GetFileFolderName(filePath)} line {lineNumber} {memberName}()]");
        }
    }
}
