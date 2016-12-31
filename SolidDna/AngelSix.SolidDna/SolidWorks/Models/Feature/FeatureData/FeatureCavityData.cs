using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Cavity feature data
    /// </summary>
    public class FeatureCavityData : SolidDnaObject<ICavityFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCavityData(ICavityFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
