using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sweep feature data
    /// </summary>
    public class FeatureSweepData : SolidDnaObject<ISweepFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSweepData(ISweepFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
