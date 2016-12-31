using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Hem feature data
    /// </summary>
    public class FeatureHemData : SolidDnaObject<IHemFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureHemData(IHemFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
