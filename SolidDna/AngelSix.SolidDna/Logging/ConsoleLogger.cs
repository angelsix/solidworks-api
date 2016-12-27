using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A logger that outputs messages to the Console window
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void Log(LogFactoryLevel level, string message)
        {
            var output = $"[{level}] {message}";
            Console.WriteLine(output);
        }
    }
}
