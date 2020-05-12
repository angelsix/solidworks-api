using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sheet Metal Normal Cut feature data
    /// </summary>
    public class FeatureNormalCutData : SolidDnaObject<ISMNormalCutFeatureData2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureNormalCutData(ISMNormalCutFeatureData2 model) : base(model)
        {

        }

        #endregion
    }
}