using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Macro feature data
    /// </summary>
    public class FeatureMacroData : SolidDnaObject<IMacroFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMacroData(IMacroFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
