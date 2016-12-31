using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Cosmetic Thread feature data
    /// </summary>
    public class FeatureCosmeticThreadData : SolidDnaObject<ICosmeticThreadFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCosmeticThreadData(ICosmeticThreadFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
