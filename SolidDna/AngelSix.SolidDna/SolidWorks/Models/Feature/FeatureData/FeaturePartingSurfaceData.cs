using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Parting Surface feature data
    /// </summary>
    public class FeaturePartingSurfaceData : SolidDnaObject<IPartingSurfaceFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeaturePartingSurfaceData(IPartingSurfaceFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
