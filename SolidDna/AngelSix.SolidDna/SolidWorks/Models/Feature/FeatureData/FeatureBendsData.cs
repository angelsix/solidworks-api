using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Bends feature data
    /// </summary>
    public class FeatureBendsData : SolidDnaObject<IBendsFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureBendsData(IBendsFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
