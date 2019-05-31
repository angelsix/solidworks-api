using System;
using SolidWorks.Interop.swconst;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// The type of template for a model, from <see cref="swDocTemplateTypes_e"/>
    /// </summary>
    [Flags]
    public enum ModelTemplateType
    {
        /// <summary>
        /// Nothing is open
        /// </summary>
        None = 1,

        /// <summary>
        /// A part is open
        /// </summary>
        Part = 2,

        /// <summary>
        /// An assembly is open
        /// </summary>
        Assembly = 4,

        /// <summary>
        /// A drawing is open
        /// </summary>
        Drawing = 8,

        /// <summary>
        /// An in-context part is open
        /// </summary>
        InContext = 16
    }
}
