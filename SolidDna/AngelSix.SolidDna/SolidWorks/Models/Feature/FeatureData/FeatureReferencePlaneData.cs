using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Reference Plane feature data
    /// </summary>
    public class FeatureReferencePlaneData : SolidDnaObject<IRefPlaneFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureReferencePlaneData(IRefPlaneFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
