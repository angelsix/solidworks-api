using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Reference Curve feature
    /// </summary>
    public class FeatureReferenceCurve : SolidDnaObject<IReferenceCurve>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureReferenceCurve(IReferenceCurve model) : base(model)
        {

        }

        #endregion
    }
}
