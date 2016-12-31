using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Extrude feature data
    /// </summary>
    public class FeatureExtrudeData : SolidDnaObject<IExtrudeFeatureData2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureExtrudeData(IExtrudeFeatureData2 model) : base(model)
        {

        }

        #endregion
    }
}
