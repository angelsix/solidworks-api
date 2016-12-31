using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Cosmetic Weld Bead feature data
    /// </summary>
    public class FeatureCosmeticWeldBeadData : SolidDnaObject<ICosmeticWeldBeadFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCosmeticWeldBeadData(ICosmeticWeldBeadFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
