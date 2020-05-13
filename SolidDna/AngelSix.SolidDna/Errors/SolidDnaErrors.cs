using Dna;
using System;
using System.Threading.Tasks;
using static Dna.FrameworkDI;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Provides details and lookups for SolidDNA errors and codes
    /// </summary>
    public static class SolidDnaErrors
    {
        #region Public Methods

        /// <summary>
        /// Creates a <see cref="SolidDnaError"/> from the given details
        /// </summary>
        /// <param name="errorDetails">Specific details about this exact error</param>
        /// <param name="errorTypeCode">The error type code of this error</param>
        /// <param name="errorCode">The specific error code of this error</param>
        /// <param name="innerException">If an inner exception is supplied, its message is appended to the errorDetails</param>
        /// <returns></returns>
        public static SolidDnaError CreateError(SolidDnaErrorTypeCode errorTypeCode, SolidDnaErrorCode errorCode, string errorDetails, Exception innerException = null)
        {
            // Create the error
            var error = new SolidDnaError
            {
                ErrorCodeValue = errorCode,
                ErrorMessage = errorDetails,
                ErrorTypeCodeValue = errorTypeCode,
            };

            // Set inner details
            if (innerException != null)
                error.ErrorDetails = $"{error.ErrorDetails}. Inner Exception ({innerException.GetType()}: {innerException.GetErrorMessage()}";

            return error;
        }

        /// <summary>
        /// Runs an action and catches any exceptions thrown
        /// wrapping and rethrowing them as a <see cref="SolidDnaException"/>
        /// </summary>
        /// <param name="action">The action to run</param>
        /// <param name="errorTypeCode">The <see cref="SolidDnaErrorTypeCode"/> to wrap the exception in</param>
        /// <param name="errorCode">The <see cref="SolidDnaErrorCode"/> to wrap the exception in</param>
        /// <param name="errorDescription">The description of the error if thrown</param>
        public static void Wrap(Action action, SolidDnaErrorTypeCode errorTypeCode, SolidDnaErrorCode errorCode, string errorDescription)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                // Create the SolidDNA exception
                var error = new SolidDnaException(SolidDnaErrors.CreateError(
                    errorTypeCode,
                    errorCode,
                    errorDescription), ex);

                // If it should just be logged and ignored, log it
                if (SolidDnaEnvironment.LogAndIgnoreUncaughtExceptions)
                {
                    // Log the error
                    Logger?.LogCriticalSource($"SolidDNA Exception created. {error.SolidDnaError?.ToString()}");
                    if (error.InnerException != null)
                        Logger?.LogCriticalSource($"Inner Exception: { error.InnerException.GetErrorMessage()}");
                }
                // Otherwise, throw 
                else
                    throw error;
            }
        }

        /// <summary>
        /// Runs a function and catches any exceptions thrown, 
        /// wrapping and rethrowing them as a <see cref="SolidDnaException"/>
        /// </summary>
        /// <param name="func">The function to run</param>
        /// <param name="errorTypeCode">The <see cref="SolidDnaErrorTypeCode"/> to wrap the exception in</param>
        /// <param name="errorCode">The <see cref="SolidDnaErrorCode"/> to wrap the exception in</param>
        /// <param name="errorDescription">The description of the error if thrown</param>
        public static T Wrap<T>(Func<T> func, SolidDnaErrorTypeCode errorTypeCode, SolidDnaErrorCode errorCode, string errorDescription)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                // Create the SolidDNA exception
                var error = new SolidDnaException(SolidDnaErrors.CreateError(
                    errorTypeCode,
                    errorCode,
                    errorDescription), ex);

                // If it should just be logged and ignored, log it
                if (SolidDnaEnvironment.LogAndIgnoreUncaughtExceptions)
                {
                    // Log the error
                    Logger?.LogCriticalSource($"SolidDNA Exception created. {error.SolidDnaError?.ToString()}");
                    if (error.InnerException != null)
                        Logger?.LogCriticalSource($"Inner Exception: { error.InnerException.GetErrorMessage()}");

                    return default;
                }
                // Otherwise, throw 
                else
                    throw error;
            }
        }

        /// <summary>
        /// Runs a task and catches any exceptions thrown, 
        /// wrapping and rethrowing them as a <see cref="SolidDnaException"/>
        /// </summary>
        /// <param name="task">The task to run</param>
        /// <param name="errorTypeCode">The <see cref="SolidDnaErrorTypeCode"/> to wrap the exception in</param>
        /// <param name="errorCode">The <see cref="SolidDnaErrorCode"/> to wrap the exception in</param>
        /// <param name="errorDescription">The description of the error if thrown</param>
        public static async Task WrapAwaitAsync(Func<Task> task, SolidDnaErrorTypeCode errorTypeCode, SolidDnaErrorCode errorCode, string errorDescription)
        {
            try
            {
                await task();
            }
            catch (Exception ex)
            {
                // Create the SolidDNA exception
                var error = new SolidDnaException(SolidDnaErrors.CreateError(
                    errorTypeCode,
                    errorCode,
                    errorDescription), ex);

                // If it should just be logged and ignored, log it
                if (SolidDnaEnvironment.LogAndIgnoreUncaughtExceptions)
                {
                    // Log the error
                    Logger?.LogCriticalSource($"SolidDNA Exception created. {error.SolidDnaError?.ToString()}");
                    if (error.InnerException != null)
                        Logger?.LogCriticalSource($"Inner Exception: { error.InnerException.GetErrorMessage()}");
                }
                // Otherwise, throw 
                else
                    throw error;
            }
        }

        /// <summary>
        /// Runs a task and catches any exceptions thrown
        /// wrapping and rethrowing them as a <see cref="SolidDnaException"/>
        /// 
        /// Returns the result of the task
        /// </summary>
        /// <param name="task">The task to run</param>
        /// <param name="errorTypeCode">The <see cref="SolidDnaErrorTypeCode"/> to wrap the exception in</param>
        /// <param name="errorCode">The <see cref="SolidDnaErrorCode"/> to wrap the exception in</param>
        /// <param name="errorDescription">The description of the error if thrown</param>
        public static async Task<T> WrapAwaitAsync<T>(Func<Task<T>> task, SolidDnaErrorTypeCode errorTypeCode, SolidDnaErrorCode errorCode, string errorDescription)
        {
            try
            {
                return await task();
            }
            catch (Exception ex)
            {
                // Create the SolidDNA exception
                var error = new SolidDnaException(SolidDnaErrors.CreateError(
                    errorTypeCode,
                    errorCode,
                    errorDescription), ex);

                // If it should just be logged and ignored, log it
                if (SolidDnaEnvironment.LogAndIgnoreUncaughtExceptions)
                {
                    // Log the error
                    Logger?.LogCriticalSource($"SolidDNA Exception created. {error.SolidDnaError?.ToString()}");
                    if (error.InnerException != null)
                        Logger?.LogCriticalSource($"Inner Exception: { error.InnerException.GetErrorMessage()}");

                    // Return a default object
                    return default;
                }
                // Otherwise, throw it up
                else
                    throw error;
            }
        }


        #endregion
    }
}
