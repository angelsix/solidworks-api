using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Parallel Mate feature data
    /// </summary>
    public class FeatureParallelMateData : SolidDnaObject<IParallelMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureParallelMateData(IParallelMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}