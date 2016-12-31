using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Mate feature
    /// </summary>
    public class FeatureMate : SolidDnaObject<IMate2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMate(IMate2 model) : base(model)
        {

        }

        #endregion
    }
}
