using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Fill feature data
    /// </summary>
    public class FeatureSurfaceFillData : SolidDnaObject<IFillSurfaceFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceFillData(IFillSurfaceFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
