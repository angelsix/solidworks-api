using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Table Pattern feature data
    /// </summary>
    public class FeatureTablePatternData : SolidDnaObject<ITablePatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureTablePatternData(ITablePatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
