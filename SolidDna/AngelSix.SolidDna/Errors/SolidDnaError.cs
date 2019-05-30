using System.Runtime.CompilerServices;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Details about an error that has occurred on SolidDNA
    /// </summary>
    public class SolidDnaError
    {
        #region Public Properties

        /// <summary>
        /// A generic error message for this type of error
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// A descriptive error message about this specific error
        /// </summary>
        public string ErrorDetails { get; set; }

        /// <summary>
        /// The error type code for this error
        /// </summary>
        public int ErrorTypeCode => (int)ErrorTypeCodeValue;

        /// <summary>
        /// The enum value that this error type code refers to. See <see cref="SolidDnaErrorTypeCode"/>
        /// </summary>
        public SolidDnaErrorTypeCode ErrorTypeCodeValue { get; set; }

        /// <summary>
        /// The unique error code for this exact error
        /// </summary>
        public int ErrorCode => (int)ErrorCodeValue;

        /// <summary>
        /// The enum value that this error code refers to. See <see cref="SolidDnaErrorCode"/>
        /// </summary>
        public SolidDnaErrorCode ErrorCodeValue { get; set; }

        /// <summary>
        /// The member name of the calling / creating method.
        /// </summary>
        public string CallerMemberName { get; set; }

        /// <summary>
        /// The file path to the calling / creating class.
        /// </summary>
        public string CallerFilePath { get; set; }

        /// <summary>
        /// The line number of the calling / creating class.
        /// </summary>
        public int CallerLineNumber { get; set; }

        /// <summary>
        /// Any relevant data to the error message in human readable form;
        /// such as a file path or URI
        /// </summary>
        public string PertinentData { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SolidDnaError([CallerMemberName] string callerMemberName = "", [CallerFilePath]string callerFilePath = "", [CallerLineNumber]int callerLineNumber = 0)
        {
            // Set the caller information
            CallerMemberName = callerMemberName;
            CallerFilePath = callerFilePath;
            CallerLineNumber = callerLineNumber;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="solidDnaError">The error to clone from</param>
        public SolidDnaError(SolidDnaError solidDnaError)
        {
            ErrorTypeCodeValue = solidDnaError.ErrorTypeCodeValue;
            ErrorDetails = solidDnaError.ErrorDetails;
            ErrorMessage = solidDnaError.ErrorMessage;
            ErrorCodeValue = solidDnaError.ErrorCodeValue;
            PertinentData = solidDnaError.PertinentData;
            CallerMemberName = solidDnaError.CallerMemberName;
            CallerFilePath = solidDnaError.CallerFilePath;
            CallerLineNumber = solidDnaError.CallerLineNumber;
        }

        #endregion

        /// <summary>
        /// Returns a user-friendly string about the error
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{ErrorDetails} ({ErrorMessage}) [T{ErrorTypeCode} E{ErrorCode}]";
        }
    }
}
