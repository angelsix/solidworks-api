using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Reference Point feature
    /// </summary>
    public class FeatureRefPoint : SolidDnaObject<IRefPoint>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureRefPoint(IRefPoint model) : base(model)
        {

        }

        #endregion
    }
}
