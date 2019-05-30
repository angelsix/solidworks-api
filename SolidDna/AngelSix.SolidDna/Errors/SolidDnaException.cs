using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A SolidDNA exception containing specific error codes and details for a <see cref="SolidDnaError"/>
    /// </summary>
    public class SolidDnaException : Exception
    {
        #region Public Properties

        /// <summary>
        /// The SolidDNA error details
        /// </summary>
        public SolidDnaError SolidDnaError { get; set; }

        /// <summary>
        /// The inner exception details
        /// </summary>
        public new Exception InnerException { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SolidDnaException(SolidDnaError error, Exception innerException = null)
        {
            SolidDnaError = error;
            InnerException = innerException;
        }

        #endregion

        /// <summary>
        /// Combines the SolidDnaError and InnerException, adding the InnerException message to the SolidDnaError.ErrorDescription
        /// </summary>
        /// <returns></returns>
        public SolidDnaError ToCompositeSolidDnaError()
        {
            var copy = new SolidDnaError(SolidDnaError);

            if (InnerException != null)
                copy.ErrorDetails = $"{copy.ErrorDetails}. Inner Exception ({InnerException.GetType()}): {InnerException.GetErrorMessage()}";

            return copy;
        }
    }
}
