using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sketch Pattern feature data
    /// </summary>
    public class FeatureSketchPatternData : SolidDnaObject<ISketchPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSketchPatternData(ISketchPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
