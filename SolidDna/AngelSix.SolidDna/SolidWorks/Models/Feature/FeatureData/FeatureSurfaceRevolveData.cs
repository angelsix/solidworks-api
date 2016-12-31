using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Revolve feature data
    /// </summary>
    public class FeatureSurfaceRevolveData : SolidDnaObject<ISurfRevolveFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceRevolveData(ISurfRevolveFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
