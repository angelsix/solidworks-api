using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Broken Out Section feature data
    /// </summary>
    public class FeatureBrokenOutSectionData : SolidDnaObject<IBrokenOutSectionFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureBrokenOutSectionData(IBrokenOutSectionFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
