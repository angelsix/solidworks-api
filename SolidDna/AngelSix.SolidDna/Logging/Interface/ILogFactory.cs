using System;
using System.Collections.Generic;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A logger to log application wide events from any area of the application
    /// </summary>
    public interface ILogFactory
    {
        /// <summary>
        /// Called when a new log message is received
        /// </summary>
        event Action<string, LogFactoryLevel> NewLog;

        /// <summary>
        /// Contains a history of the most recent log events in order or date received
        /// </summary>
        List<LogDetails> LogHistory { get; }

        /// <summary>
        /// The current level to log messages at
        /// </summary>
        LogFactoryLevel LogLevel { get; set; }

        /// <summary>
        /// If true, the name of the method that called the Log function is added to the message
        /// </summary>
        bool LogMethodName { get; set; }

        /// <summary>
        /// If <see cref="LogMethodName"/> is true, this is a string.Format string specifying where to place the Method name. 0 is the message, 1 is the method
        /// For example "{0} ({1})" with a log message called from MyFunction and a message of "Hello World" would output:
        /// "Hello World (MyFunction)"
        /// </summary>
        string LogMethodNameFormat { get; set; }

        /// <summary>
        /// Add a logger to the factory to be called for every log message
        /// </summary>
        /// <param name="logger">The logger to add</param>
        void AddLogger(ILogger logger);

        /// <summary>
        /// Remove a logger from the factory
        /// </summary>
        /// <param name="logger">The logger to remove</param>
        void RemoveLogger(ILogger logger);

        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The level of this log message</param>
        /// <param name="origin">The origin of this message, usually the calling function or class</param>
        void Log(string message, LogFactoryLevel level = LogFactoryLevel.Information, string origin = "");
    }
}
