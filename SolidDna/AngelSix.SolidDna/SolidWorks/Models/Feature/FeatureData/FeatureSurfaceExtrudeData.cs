using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Extrude feature data
    /// </summary>
    public class FeatureSurfaceExtrudeData : SolidDnaObject<ISurfExtrudeFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceExtrudeData(ISurfExtrudeFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
