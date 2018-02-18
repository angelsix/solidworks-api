using System;
using SolidWorks.Interop.swconst;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Version of a particular format to save the document, from <see cref="swSaveAsVersion_e"/>
    /// </summary>
    public enum SaveAsVersion
    {
        /// <summary>
        /// Saves the model in the default way with no special settings (this is the typical way)
        /// </summary>
        CurrentVersion = 0,

        /// <summary>
        /// Obsolete and no longer supported
        /// </summary>
        [Obsolete("Use CurrentVersion instead")]
        SolidWorks98plus = 1,

        /// <summary>
        /// Saves the model in Pro/E format
        /// </summary>
        FormatProE = 2,

        /// <summary>
        /// Saves a detached drawing as a standard drawing
        /// </summary>
        StandardDrawing = 3,

        /// <summary>
        /// Saves a standard drawing as a detached drawing
        /// </summary>
        DetachedDrawing = 4
    }
}
