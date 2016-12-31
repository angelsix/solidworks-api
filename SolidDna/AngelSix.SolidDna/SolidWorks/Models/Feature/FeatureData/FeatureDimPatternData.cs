using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Dim Pattern feature data
    /// </summary>
    public class FeatureDimPatternData : SolidDnaObject<IDimPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureDimPatternData(IDimPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
