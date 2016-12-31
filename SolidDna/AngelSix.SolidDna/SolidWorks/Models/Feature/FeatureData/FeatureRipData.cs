using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Rip feature data
    /// </summary>
    public class FeatureRipData : SolidDnaObject<IRipFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureRipData(IRipFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
