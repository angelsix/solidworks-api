using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Weldment Trim Extend feature data
    /// </summary>
    public class FeatureWeldmentTrimExtendData : SolidDnaObject<IWeldmentTrimExtendFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureWeldmentTrimExtendData(IWeldmentTrimExtendFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
