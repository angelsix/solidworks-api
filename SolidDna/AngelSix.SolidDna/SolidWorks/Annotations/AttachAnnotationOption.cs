using SolidWorks.Interop.swconst;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// The options for the <see cref="DrawingDocument.AttachAnnotation"/> call
    /// from <see cref="swAttachAnnotationOption_e"/>
    /// </summary>
    public enum AttachAnnotationOption
    {
        /// <summary>
        /// Attaches the selected annotation to the sheet
        /// </summary>
        Sheet = 1,

        /// <summary>
        /// Attaches the selected annotation to the view
        /// </summary>
        View = 2
    }
}
