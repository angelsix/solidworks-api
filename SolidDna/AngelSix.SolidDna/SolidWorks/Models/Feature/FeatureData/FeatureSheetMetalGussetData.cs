using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sheet Metal Gusset feature data
    /// </summary>
    public class FeatureSheetMetalGussetData : SolidDnaObject<ISMGussetFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSheetMetalGussetData(ISMGussetFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
