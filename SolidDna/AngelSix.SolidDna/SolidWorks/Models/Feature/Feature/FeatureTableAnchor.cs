using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Table Anchor feature
    /// </summary>
    public class FeatureTableAnchor : SolidDnaObject<ITableAnchor>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureTableAnchor(ITableAnchor model) : base(model)
        {

        }

        #endregion
    }
}
