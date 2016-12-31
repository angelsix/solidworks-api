using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Local Curve Pattern feature data
    /// </summary>
    public class FeatureLocalCurvePatternData : SolidDnaObject<ILocalCurvePatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureLocalCurvePatternData(ILocalCurvePatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
