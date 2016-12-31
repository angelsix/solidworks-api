using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Draft feature data
    /// </summary>
    public class FeatureDraftData : SolidDnaObject<IDraftFeatureData2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureDraftData(IDraftFeatureData2 model) : base(model)
        {

        }

        #endregion
    }
}
