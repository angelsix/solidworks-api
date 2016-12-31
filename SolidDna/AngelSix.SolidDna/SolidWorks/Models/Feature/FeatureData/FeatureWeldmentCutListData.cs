using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Weldment Cut List feature data
    /// </summary>
    public class FeatureWeldmentCutListData : SolidDnaObject<IWeldmentCutListFeature>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureWeldmentCutListData(IWeldmentCutListFeature model) : base(model)
        {

        }

        #endregion
    }
}
