using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Trim feature data
    /// </summary>
    public class FeatureSurfaceTrimData : SolidDnaObject<ISurfaceTrimFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceTrimData(ISurfaceTrimFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
