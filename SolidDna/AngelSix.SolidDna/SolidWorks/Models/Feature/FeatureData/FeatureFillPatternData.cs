using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Fill Pattern feature data
    /// </summary>
    public class FeatureFillPatternData : SolidDnaObject<IFillPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureFillPatternData(IFillPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
