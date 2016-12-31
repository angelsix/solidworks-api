using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Flat Pattern feature data
    /// </summary>
    public class FeatureFlatPatternData : SolidDnaObject<IFlatPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureFlatPatternData(IFlatPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
