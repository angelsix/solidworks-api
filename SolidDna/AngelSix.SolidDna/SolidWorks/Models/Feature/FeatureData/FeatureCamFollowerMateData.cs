using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Cam Follower Mate feature data
    /// </summary>
    public class FeatureCamFollowerMateData : SolidDnaObject<ICamFollowerMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCamFollowerMateData(ICamFollowerMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}