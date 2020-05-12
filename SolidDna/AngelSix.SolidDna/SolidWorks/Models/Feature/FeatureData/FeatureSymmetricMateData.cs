using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Symmetric Mate feature data
    /// </summary>
    public class FeatureSymmetricMateData : SolidDnaObject<ISymmetricMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSymmetricMateData(ISymmetricMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}