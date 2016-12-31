using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Folds feature data
    /// </summary>
    public class FeatureFoldsData : SolidDnaObject<IFoldsFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureFoldsData(IFoldsFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
