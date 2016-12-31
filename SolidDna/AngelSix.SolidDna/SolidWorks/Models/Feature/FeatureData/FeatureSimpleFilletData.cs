using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Simple Fillet feature data
    /// </summary>
    public class FeatureSimpleFilletData : SolidDnaObject<ISimpleFilletFeatureData2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSimpleFilletData(ISimpleFilletFeatureData2 model) : base(model)
        {

        }

        #endregion
    }
}
