using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Chamfer feature data
    /// </summary>
    public class FeatureChamferData : SolidDnaObject<IChamferFeatureData2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureChamferData(IChamferFeatureData2 model) : base(model)
        {

        }

        #endregion
    }
}
