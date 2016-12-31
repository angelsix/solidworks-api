using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Composite Curve feature data
    /// </summary>
    public class FeatureCompositeCurveData : SolidDnaObject<ICompositeCurveFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCompositeCurveData(ICompositeCurveFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
