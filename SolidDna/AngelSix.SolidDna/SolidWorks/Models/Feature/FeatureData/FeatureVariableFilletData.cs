using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Variable Fillet feature data
    /// </summary>
    public class FeatureVariableFilletData : SolidDnaObject<IVariableFilletFeatureData2>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureVariableFilletData(IVariableFilletFeatureData2 model) : base(model)
        {

        }

        #endregion
    }
}
