using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sheet Metal feature data
    /// </summary>
    public class FeatureSheetMetalData : SolidDnaObject<ISheetMetalFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSheetMetalData(ISheetMetalFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
