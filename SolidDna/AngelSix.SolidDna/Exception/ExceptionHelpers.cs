using Dna;
using System;
using System.Diagnostics;
using static Dna.FrameworkDI;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A set of helper functions related to exceptions
    /// </summary>
    public static class ExceptionHelpers
    {
        /// <summary>
        /// A tidy way to do try { ... } finally { ... }
        /// </summary>
        /// <param name="action">The action to perform in the try block</param>
        /// <param name="final">The action to perform in the finally block</param>
        public static void TryFinally(Action action, Action final)
        {
            try
            {
                action();
            }
            finally
            {
                final();
            }
        }

        /// <summary>
        /// Combines the exception itself and all inner exceptions into one string 
        /// so the full details of the error can be easily presented.
        /// </summary>
        /// <param name="ex">The exception to retrieve the inner exception details from</param>
        /// <returns></returns>
        public static string GetErrorMessage(this Exception ex)
        {
            var text = string.Empty;

            while (ex != null)
            {
                text = ex + Environment.NewLine + text;

                ex = ex.InnerException;
            }

            return text;
        }

        /// <summary>
        /// Handles the exception and logs it to the injected logger
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="source">The source of the exception (usually the method or class name)</param>
        public static void Handle(this Exception ex, string source)
        {
            try
            {
                // Break here if we are debugging
                if (Debugger.IsAttached)
                    Debugger.Break();

                // Log the error
                Logger.LogCriticalSource($"Unexpected error at {source}. {ex.GetErrorMessage()}");
            }
            catch (Exception iex)
            {
                Logger.LogCriticalSource("GLOBAL EXCEPTION CRASHED ITSELF WITH " + iex.GetErrorMessage());
            }
        }
    }
}
