using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Free Point Curve feature data
    /// </summary>
    public class FeatureFreePointCurveData : SolidDnaObject<IFreePointCurveFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureFreePointCurveData(IFreePointCurveFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
