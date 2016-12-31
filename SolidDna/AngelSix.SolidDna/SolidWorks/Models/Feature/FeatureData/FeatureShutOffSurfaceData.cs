using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Shut Off Surface feature data
    /// </summary>
    public class FeatureShutOffSurfaceData : SolidDnaObject<IShutOffSurfaceFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureShutOffSurfaceData(IShutOffSurfaceFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
