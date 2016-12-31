using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Bom feature
    /// </summary>
    public class FeatureBom : SolidDnaObject<IBomFeature>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureBom(IBomFeature model) : base(model)
        {

        }

        #endregion
    }
}
