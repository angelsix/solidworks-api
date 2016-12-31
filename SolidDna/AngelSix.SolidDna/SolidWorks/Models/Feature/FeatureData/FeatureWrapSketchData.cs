using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Wrap Sketch feature data
    /// </summary>
    public class FeatureWrapSketchData : SolidDnaObject<IWrapSketchFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureWrapSketchData(IWrapSketchFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
