using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Projection Curve feature data
    /// </summary>
    public class FeatureProjectionCurveData : SolidDnaObject<IProjectionCurveFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureProjectionCurveData(IProjectionCurveFeatureData model) : base(model)
        {
            
        }

        #endregion
    }
}
