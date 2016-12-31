using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Tooling Split feature data
    /// </summary>
    public class FeatureToolingSplitData : SolidDnaObject<IToolingSplitFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureToolingSplitData(IToolingSplitFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
