using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Indent feature data
    /// </summary>
    public class FeatureIndentData : SolidDnaObject<IIndentFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureIndentData(IIndentFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
