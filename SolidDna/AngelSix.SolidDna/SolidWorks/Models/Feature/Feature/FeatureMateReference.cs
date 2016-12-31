using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Mate Reference feature
    /// </summary>
    public class FeatureMateReference : SolidDnaObject<IMateReference>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMateReference(IMateReference model) : base(model)
        {

        }

        #endregion
    }
}
