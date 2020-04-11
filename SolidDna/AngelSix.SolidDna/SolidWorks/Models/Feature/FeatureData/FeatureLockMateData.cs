using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Lock Mate feature data
    /// </summary>
    public class FeatureLockMateData : SolidDnaObject<ILockMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureLockMateData(ILockMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}