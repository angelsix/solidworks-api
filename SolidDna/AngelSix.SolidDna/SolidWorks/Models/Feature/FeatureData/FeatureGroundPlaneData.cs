using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Ground Plane feature data
    /// </summary>
    public class FeatureGroundPlaneData : SolidDnaObject<IGroundPlaneFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureGroundPlaneData(IGroundPlaneFeatureData model) : base(model)
        {

        }

        #endregion
    }
}