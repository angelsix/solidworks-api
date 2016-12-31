using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Reference Point Curve feature data
    /// </summary>
    public class FeatureReferencePointCurveData : SolidDnaObject<IReferencePointCurveFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureReferencePointCurveData(IReferencePointCurveFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
