using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Derived Pattern feature data
    /// </summary>
    public class FeatureDerivedPatternData : SolidDnaObject<IDerivedPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureDerivedPatternData(IDerivedPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
