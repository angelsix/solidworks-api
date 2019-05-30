using SolidWorks.Interop.swmotionstudy;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Motion Study Results feature
    /// </summary>
    public class FeatureMotionStudyResults : SolidDnaObject<IMotionStudyResults>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMotionStudyResults(IMotionStudyResults model) : base(model)
        {

        }

        #endregion
    }
}
