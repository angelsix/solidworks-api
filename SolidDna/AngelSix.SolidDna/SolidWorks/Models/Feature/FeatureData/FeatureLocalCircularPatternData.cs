using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Local Circular Pattern feature data
    /// </summary>
    public class FeatureLocalCircularPatternData : SolidDnaObject<ILocalCircularPatternFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureLocalCircularPatternData(ILocalCircularPatternFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
