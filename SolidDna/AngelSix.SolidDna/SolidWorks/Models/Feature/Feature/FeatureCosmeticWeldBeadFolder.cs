using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Cosmetic Weld Bead Folder feature
    /// </summary>
    public class FeatureCosmeticWeldBeadFolder : SolidDnaObject<ICosmeticWeldBeadFolder>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCosmeticWeldBeadFolder(ICosmeticWeldBeadFolder model) : base(model)
        {

        }

        #endregion
    }
}
