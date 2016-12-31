using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Camera feature
    /// </summary>
    public class FeatureCamera : SolidDnaObject<ICamera>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCamera(ICamera model) : base(model)
        {

        }

        #endregion
    }
}
