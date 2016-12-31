using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sketch Block Instance feature
    /// </summary>
    public class FeatureSketchBlockInstance : SolidDnaObject<ISketchBlockInstance>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSketchBlockInstance(ISketchBlockInstance model) : base(model)
        {

        }

        #endregion
    }
}
