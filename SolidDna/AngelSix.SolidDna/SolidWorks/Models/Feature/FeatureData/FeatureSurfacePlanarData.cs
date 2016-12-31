using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Planar feature data
    /// </summary>
    public class FeatureSurfacePlanarData : SolidDnaObject<ISurfacePlanarFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfacePlanarData(ISurfacePlanarFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
