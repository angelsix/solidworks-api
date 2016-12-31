using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Replace Face feature data
    /// </summary>
    public class FeatureReplaceFaceData : SolidDnaObject<IReplaceFaceFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureReplaceFaceData(IReplaceFaceFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
