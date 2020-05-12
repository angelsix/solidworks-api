using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Screw Mate feature data
    /// </summary>
    public class FeatureScrewMateData : SolidDnaObject<IScrewMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureScrewMateData(IScrewMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}