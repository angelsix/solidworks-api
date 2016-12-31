using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Chain Pattern feature data
    /// </summary>
    public class FeatureChainPatternData : SolidDnaObject<IChainPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureChainPatternData(IChainPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
