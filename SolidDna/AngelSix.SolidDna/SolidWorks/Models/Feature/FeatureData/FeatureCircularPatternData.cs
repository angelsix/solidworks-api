using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Circular Pattern feature data
    /// </summary>
    public class FeatureCircularPatternData : SolidDnaObject<ICircularPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCircularPatternData(ICircularPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
