using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Detail Circle feature
    /// </summary>
    public class FeatureDetailCircle : SolidDnaObject<IDetailCircle>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureDetailCircle(IDetailCircle model) : base(model)
        {

        }

        #endregion
    }
}
