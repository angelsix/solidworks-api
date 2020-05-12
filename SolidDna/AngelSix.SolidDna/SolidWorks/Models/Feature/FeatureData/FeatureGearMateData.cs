using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Gear Mate feature data
    /// </summary>
    public class FeatureGearMateData : SolidDnaObject<IGearMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureGearMateData(IGearMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}