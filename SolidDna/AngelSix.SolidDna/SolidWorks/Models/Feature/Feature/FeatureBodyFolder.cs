using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Body Folder feature
    /// </summary>
    public class FeatureBodyFolder : SolidDnaObject<IBodyFolder>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureBodyFolder(IBodyFolder model) : base(model)
        {

        }

        #endregion
    }
}
