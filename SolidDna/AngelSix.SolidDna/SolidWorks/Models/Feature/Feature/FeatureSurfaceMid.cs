using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Surface Mid feature
    /// </summary>
    public class FeatureSurfaceMid : SolidDnaObject<IMidSurface3>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSurfaceMid(IMidSurface3 model) : base(model)
        {

        }

        #endregion
    }
}
