using System.Diagnostics;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A logger that outputs messages to the Trace
    /// </summary>
    public class TraceLogger : ILogger
    {
        public void Log(LogFactoryLevel level, string message)
        {
            var output = $"[{level}] {message}";
            Trace.WriteLine(output);
        }
    }
}
