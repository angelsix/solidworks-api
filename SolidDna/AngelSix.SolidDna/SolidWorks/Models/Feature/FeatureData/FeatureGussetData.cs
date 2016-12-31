using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Gusset feature data
    /// </summary>
    public class FeatureGussetData : SolidDnaObject<IGussetFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureGussetData(IGussetFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
