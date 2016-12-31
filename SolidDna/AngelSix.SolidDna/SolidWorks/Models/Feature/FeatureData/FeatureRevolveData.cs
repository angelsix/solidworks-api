using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Revolve feature data
    /// </summary>
    public class FeatureRevolveData : SolidDnaObject<IRevolveFeatureData2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureRevolveData(IRevolveFeatureData2 model) : base(model)
        {

        }

        #endregion
    }
}
