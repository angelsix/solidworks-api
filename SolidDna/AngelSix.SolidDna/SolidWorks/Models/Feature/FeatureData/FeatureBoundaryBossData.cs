using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Boundary Boss feature data
    /// </summary>
    public class FeatureBoundaryBossData : SolidDnaObject<IBoundaryBossFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureBoundaryBossData(IBoundaryBossFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
