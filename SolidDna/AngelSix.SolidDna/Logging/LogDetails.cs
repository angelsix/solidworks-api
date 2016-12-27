using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Details about a log message
    /// </summary>
    public class LogDetails
    {
        /// <summary>
        /// The message for this log entry
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The date this message was logged
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The level of severity for this entry
        /// </summary>
        public LogFactoryLevel Level { get; set; }
    }
}
