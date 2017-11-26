using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A basic log factory that comes pre-installed with a Debug.WriteLine logger
    /// </summary>
    public class BaseLogFactory : ILogFactory
    {
        #region Private Members

        private List<ILogger> mLoggers = new List<ILogger>();

        private SemaphoreSlim mSelfLock = new SemaphoreSlim(1,1);
        private List<LogDetails> mLogHistory = new List<LogDetails>();

        #endregion

        #region Public Properties
        
        /// <summary>
        /// The severity level of this log
        /// </summary>
        public LogFactoryLevel LogLevel { get; set; }

        /// <summary>
        /// A list of historical logs
        /// </summary>
        public List<LogDetails> LogHistory { get { return SafeWait<List<LogDetails>>(() => mLogHistory); } }

        /// <summary>
        /// The method name associated with this log
        /// </summary>
        public bool LogMethodName { get; set; }

        /// <summary>
        /// The string format to use when outputting the log message
        /// {0} is the message, {1} is the origin (the method that created the log)
        /// </summary>
        /// <example>
        /// LogMethodNameFormat: "{0} --{1}"
        /// From inside MyFunction() => Logger.Log("Some message")
        /// returns: "Some message --MyFunction()"
        /// </example>
        public string LogMethodNameFormat { get; set; }

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when a new log is created
        /// </summary>
        public event Action<string, LogFactoryLevel> NewLog;

        #endregion

        #region .ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseLogFactory()
        {
            // Set default level to log everything 
            LogLevel = LogFactoryLevel.Verbose;

            // By default output the name of the function that called the Log method
            LogMethodName = true;

            // Default string format placing method name at end
            LogMethodNameFormat = "{0} --{1}";

            // Add the Debug, Console and TraceLogger by default
            mLoggers.Add(new DebugLogger());
            mLoggers.Add(new TraceLogger());
            mLoggers.Add(new ConsoleLogger());

            // Post the "initialized" message
            Log("Initialized", LogFactoryLevel.Verbose);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a logger to this log factory
        /// </summary>
        /// <param name="logger">The logger to add</param>
        public void AddLogger(ILogger logger)
        {
            SafeWait(() => mLoggers.Add(logger));
        }

        /// <summary>
        /// Removes a logger from this log factory, if it exists
        /// </summary>
        /// <param name="logger">The logger to remove</param>
        public void RemoveLogger(ILogger logger)
        {
            SafeWait(() =>
            {
                if (mLoggers.Contains(logger))
                    mLoggers.Remove(logger);
            });
        }

        /// <summary>
        /// Logs a new message and calls all loggers in this factory to inform them of the new log
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The level of severity for this log message. The default is Information</param>
        /// <param name="origin">The origin of this log message. Useful for tracing. The default is the method name of the caller</param>
        /// <example>
        /// From inside MyFunction() => Logger.Log("Some message")
        /// returns: "Some message --MyFunction()"
        /// </example>
        public void Log(string message, LogFactoryLevel level = LogFactoryLevel.Information, [CallerMemberName]string origin = "")
        {
            // Add method name if specified
            if (LogMethodName)
                message = string.Format(LogMethodNameFormat, message, origin);

            // Send message to all loggers
            mLoggers.ForEach(f => f.Log(level, message));

            SafeWait(() =>
            {
                // Add to list
                mLogHistory.Add(new LogDetails { Date = DateTime.UtcNow, Level = level, Message = message });

                // Prune list
                if (mLogHistory.Count > 200)
                    mLogHistory.RemoveAt(0);

            });

            // Inform listeners
            NewLog?.Invoke(message, level);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Awaits for access when no other threads are accessing this class before performing the action
        /// </summary>
        /// <param name="action">The action to perform once ready</param>
        private void SafeWait(Action action)
        {
            // Wait for access
            mSelfLock.Wait();

            ExceptionHelpers.TryFinally(
                // Perform action
                () => action(),
                // Release lock
                () => mSelfLock.Release());
        }

        /// <summary>
        /// Awaits for access when no other threads are accessing this class before performing the function and returning the value
        /// </summary>
        /// <param name="func">The function to perform once ready</param>
        private T SafeWait<T>(Func<T> func)
        {
            // Default value
            T result = default(T); 

            // Wait for access
            mSelfLock.Wait();

            ExceptionHelpers.TryFinally(
                // Perform action
                () => result = func(),
                // Release lock
                () => mSelfLock.Release());

            // Return result
            return result;
        }

        #endregion
    }
}
