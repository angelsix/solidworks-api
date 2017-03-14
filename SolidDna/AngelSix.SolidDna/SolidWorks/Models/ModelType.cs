using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// The type of SolidWorks model the <see cref="ModelDoc2"/> is, from <see cref="swDocumentTypes_e"/>
    /// </summary>
    public enum ModelType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        None = 0,
        /// <summary>
        /// SolidWorks Part
        /// </summary>
        Part = 1,
        /// <summary>
        /// SolidWorks Assembly
        /// </summary>
        Assembly = 2,
        /// <summary>
        /// SolidWorks Drawing
        /// </summary>
        Drawing = 3,
        /// <summary>
        /// SolidWorks Document Manager File
        /// </summary>
        DocumentManager = 4,
        /// <summary>
        /// External File
        /// </summary>
        ExternalFile = 5,
        /// <summary>
        /// Imported Assembly
        /// </summary>
        ImportedAssembly = 6,
        /// <summary>
        /// Imported Part
        /// </summary>
        ImportedPart = 7,
    }
}
