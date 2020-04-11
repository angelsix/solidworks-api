using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Profile Center Mate feature data
    /// </summary>
    public class FeatureProfileCenterMateData : SolidDnaObject<IProfileCenterMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureProfileCenterMateData(IProfileCenterMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}