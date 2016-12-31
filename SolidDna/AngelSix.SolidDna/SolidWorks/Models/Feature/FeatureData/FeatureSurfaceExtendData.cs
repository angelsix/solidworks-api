using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Extend feature data
    /// </summary>
    public class FeatureSurfaceExtendData : SolidDnaObject<ISurfaceExtendFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceExtendData(ISurfaceExtendFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
