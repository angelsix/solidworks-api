using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Light feature
    /// </summary>
    public class FeatureLight : SolidDnaObject<ILight>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureLight(ILight model) : base(model)
        {

        }

        #endregion
    }
}
