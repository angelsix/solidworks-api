using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Jog feature data
    /// </summary>
    public class FeatureJogData : SolidDnaObject<IJogFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureJogData(IJogFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
