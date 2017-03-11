using SolidWorks.Interop.swconst;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// The type of message box icon for a SolidWorks message, of type <see cref="swMessageBoxIcon_e"/>
    /// </summary>
    public enum SolidWorksMessageBoxIcon
    {
        /// <summary>
        /// A warning icon
        /// </summary>
        Warning = 1,

        /// <summary>
        /// An information icon
        /// </summary>
        Information = 2,

        /// <summary>
        /// A question mark icon
        /// </summary>
        Question = 3,

        /// <summary>
        /// An exclamation icon
        /// </summary>
        Stop = 4
    }
}
