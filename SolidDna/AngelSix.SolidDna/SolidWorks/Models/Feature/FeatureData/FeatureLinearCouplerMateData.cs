using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Linear Coupler Mate feature data
    /// </summary>
    public class FeatureLinearCouplerMateData : SolidDnaObject<ILinearCouplerMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureLinearCouplerMateData(ILinearCouplerMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}