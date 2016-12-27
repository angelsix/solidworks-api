using System.Diagnostics;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A logger that outputs messages to the Debug window
    /// </summary>
    public class DebugLogger : ILogger
    {
        public void Log(LogFactoryLevel level, string message)
        {
            var output = $"[{level}] {message}";
            Debug.WriteLine(output);
        }
    }
}
