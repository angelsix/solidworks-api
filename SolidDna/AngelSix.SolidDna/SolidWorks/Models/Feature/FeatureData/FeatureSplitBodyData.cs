using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Split Body feature data
    /// </summary>
    public class FeatureSplitBodyData : SolidDnaObject<ISplitBodyFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSplitBodyData(ISplitBodyFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
