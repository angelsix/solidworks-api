using SolidWorks.Interop.swconst;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// The result for a SolidWorks message, of type <see cref="swMessageBoxResult_e"/>
    /// </summary>
    public enum SolidWorksMessageBoxResult
    {
        /// <summary>
        /// Abort was clicked
        /// </summary>
        Abort = 1,

        /// <summary>
        /// Ignore was clicked
        /// </summary>
        Ignore = 2,

        /// <summary>
        /// No was clicked
        /// </summary>
        No = 3,

        /// <summary>
        /// Ok was clicked
        /// </summary>
        Ok = 4,

        /// <summary>
        /// Retry was clicked
        /// </summary>
        Retry = 5,

        /// <summary>
        /// Yes was clicked
        /// </summary>
        Yes = 6,

        /// <summary>
        /// Cancel was clicked
        /// </summary>
        Cancel = 7
    }
}
