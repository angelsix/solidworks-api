using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks End Cap feature data
    /// </summary>
    public class FeatureEndCapData : SolidDnaObject<IEndCapFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureEndCapData(IEndCapFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
