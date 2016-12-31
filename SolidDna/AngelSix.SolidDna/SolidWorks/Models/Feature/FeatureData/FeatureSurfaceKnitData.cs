using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Knit feature data
    /// </summary>
    public class FeatureSurfaceKnitData : SolidDnaObject<ISurfaceKnitFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceKnitData(ISurfaceKnitFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
