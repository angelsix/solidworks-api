using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Core feature data
    /// </summary>
    public class FeatureCoreData : SolidDnaObject<ICoreFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCoreData(ICoreFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
