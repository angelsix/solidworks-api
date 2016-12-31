using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Motion Plot Axis feature data
    /// </summary>
    public class FeatureMotionPlotAxisData : SolidDnaObject<IMotionPlotAxisFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMotionPlotAxisData(IMotionPlotAxisFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
