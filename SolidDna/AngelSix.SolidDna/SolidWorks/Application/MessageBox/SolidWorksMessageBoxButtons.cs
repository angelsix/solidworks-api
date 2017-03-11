using SolidWorks.Interop.swconst;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// The type of message box buttons for a SolidWorks message, of type <see cref="swMessageBoxBtn_e"/>
    /// </summary>
    public enum SolidWorksMessageBoxButtons
    {
        /// <summary>
        /// An Abort, Retry and Ignore button
        /// </summary>
        AbortRetryIgnore = 1,

        /// <summary>
        /// A single OK button
        /// </summary>
        Ok = 2,

        /// <summary>
        /// An OK and Cancel button
        /// </summary>
        OkCancel = 3,

        /// <summary>
        /// A single Retry button
        /// </summary>
        Retry = 4,

        /// <summary>
        /// A Yes and No button
        /// </summary>
        YesNo = 5,

        /// <summary>
        /// A Yes, No and Cancel button
        /// </summary>
        YesNoCancel = 6
    }
}
