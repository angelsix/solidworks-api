using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Concentric Mate feature data
    /// </summary>
    public class FeatureConcentricMateData : SolidDnaObject<IConcentricMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureConcentricMateData(IConcentricMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}