using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Reference Axis feature
    /// </summary>
    public class FeatureRefAxis : SolidDnaObject<IRefAxis>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureRefAxis(IRefAxis model) : base(model)
        {

        }

        #endregion
    }
}
