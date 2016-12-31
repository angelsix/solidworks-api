using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Hole Series feature data
    /// </summary>
    public class FeatureHoleSeriesData : SolidDnaObject<IHoleSeriesFeatureData2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureHoleSeriesData(IHoleSeriesFeatureData2 model) : base(model)
        {

        }

        #endregion
    }
}
