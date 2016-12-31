using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Weldment Member feature data
    /// </summary>
    public class FeatureWeldmentMemberData : SolidDnaObject<IWeldmentTrimExtendFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureWeldmentMemberData(IWeldmentTrimExtendFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
