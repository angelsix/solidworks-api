using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Local Linear Pattern feature data
    /// </summary>
    public class FeatureLocalLinearPatternData : SolidDnaObject<ILocalLinearPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureLocalLinearPatternData(ILocalLinearPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
