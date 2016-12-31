using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Simple Hole feature data
    /// </summary>
    public class FeatureSimpleHoleData : SolidDnaObject<ISimpleHoleFeatureData2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSimpleHoleData(ISimpleHoleFeatureData2 model) : base(model)
        {

        }

        #endregion
    }
}
