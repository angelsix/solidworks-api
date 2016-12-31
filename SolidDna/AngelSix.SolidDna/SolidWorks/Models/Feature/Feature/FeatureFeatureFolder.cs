using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Feature folder feature
    /// </summary>
    public class FeatureFeatureFolder : SolidDnaObject<IFeatureFolder>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureFeatureFolder(IFeatureFolder model) : base(model)
        {

        }

        #endregion
    }
}
