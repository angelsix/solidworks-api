using SolidWorks.Interop.swconst;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// The opens for a document type used in calls such as <see cref="SolidWorksApplication.OpenFile(string, bool)"/>
    /// from <see cref="swDocumentTypes_e"/>
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// No document type
        /// </summary>
        None = 0,

        /// <summary>
        /// A SolidWorks Part
        /// </summary>
        Part = 1,

        /// <summary>
        /// A SolidWorks assembly
        /// </summary>
        Assembly = 2,

        /// <summary>
        /// A SolidWorks drawing
        /// </summary>
        Drawing = 3,

        /// <summary>
        /// Unknown type with no documentation
        /// </summary>
        SDM = 4,

        /// <summary>
        /// A layout file
        /// </summary>
        Layout = 5,

        /// <summary>
        /// An imported part
        /// </summary>
        ImportedPart = 6,

        /// <summary>
        /// An imported assembly
        /// </summary>
        ImportedAssembly = 7
    }
}
