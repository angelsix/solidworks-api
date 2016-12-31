using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Imported Curve feature data
    /// </summary>
    public class FeatureImportedCurveData : SolidDnaObject<IImportedCurveFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureImportedCurveData(IImportedCurveFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
