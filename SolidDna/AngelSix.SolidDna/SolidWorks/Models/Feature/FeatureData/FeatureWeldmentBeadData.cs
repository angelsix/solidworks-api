using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Weldment Bead feature data
    /// </summary>
    public class FeatureWeldmentBeadData : SolidDnaObject<IWeldmentBeadFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureWeldmentBeadData(IWeldmentBeadFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
