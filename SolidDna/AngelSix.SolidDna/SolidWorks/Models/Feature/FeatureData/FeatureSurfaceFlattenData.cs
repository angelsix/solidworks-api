using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Flatten feature data
    /// </summary>
    public class FeatureSurfaceFlattenData : SolidDnaObject<ISurfaceFlattenFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceFlattenData(ISurfaceFlattenFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
