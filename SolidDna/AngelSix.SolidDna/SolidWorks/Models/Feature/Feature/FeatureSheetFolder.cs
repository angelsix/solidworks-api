using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sheet Metal folder
    /// </summary>
    public class FeatureSheetMetalFolder: SolidDnaObject<ISheetMetalFolder>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSheetMetalFolder(ISheetMetalFolder model) : base(model)
        {

        }

        #endregion
    }
}
