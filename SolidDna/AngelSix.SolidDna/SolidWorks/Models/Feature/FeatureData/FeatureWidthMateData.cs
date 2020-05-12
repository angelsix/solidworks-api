using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Width Mate feature data
    /// </summary>
    public class FeatureWidthMateData : SolidDnaObject<IWidthMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureWidthMateData(IWidthMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}