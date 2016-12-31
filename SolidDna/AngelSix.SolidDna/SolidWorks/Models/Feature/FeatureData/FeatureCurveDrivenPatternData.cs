using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Curve Pattern feature data
    /// </summary>
    public class FeatureCurveDrivenPatternData : SolidDnaObject<ICurveDrivenPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCurveDrivenPatternData(ICurveDrivenPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
