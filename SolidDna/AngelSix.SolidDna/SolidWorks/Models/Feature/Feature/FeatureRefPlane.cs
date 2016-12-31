using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Reference Plane feature
    /// </summary>
    public class FeatureRefPlane : SolidDnaObject<IRefPlane>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureRefPlane(IRefPlane model) : base(model)
        {

        }

        #endregion
    }
}
