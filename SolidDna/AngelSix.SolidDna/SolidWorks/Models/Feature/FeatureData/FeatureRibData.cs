using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Rib feature data
    /// </summary>
    public class FeatureRibData : SolidDnaObject<IRibFeatureData2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureRibData(IRibFeatureData2 model) : base(model)
        {

        }

        #endregion
    }
}
