using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Coincident Mate feature data
    /// </summary>
    public class FeatureCoincidentMateData : SolidDnaObject<ICoincidentMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureCoincidentMateData(ICoincidentMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}