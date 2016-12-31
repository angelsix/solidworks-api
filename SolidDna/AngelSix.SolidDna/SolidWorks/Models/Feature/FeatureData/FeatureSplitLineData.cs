using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Split Line feature data
    /// </summary>
    public class FeatureSplitLineData : SolidDnaObject<ISplitLineFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSplitLineData(ISplitLineFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
