using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Motion Plot feature data
    /// </summary>
    public class FeatureMotionPlotData : SolidDnaObject<IMotionPlotFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMotionPlotData(IMotionPlotFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
