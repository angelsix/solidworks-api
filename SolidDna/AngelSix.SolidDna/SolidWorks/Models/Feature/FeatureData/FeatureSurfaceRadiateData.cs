using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Radiate feature data
    /// </summary>
    public class FeatureSurfaceRadiateData : SolidDnaObject<ISurfaceRadiateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceRadiateData(ISurfaceRadiateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
