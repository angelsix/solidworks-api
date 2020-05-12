using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Local Sketch Pattern feature data
    /// </summary>
    public class FeatureLocalSketchPatternData : SolidDnaObject<ILocalSketchPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureLocalSketchPatternData(ILocalSketchPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}