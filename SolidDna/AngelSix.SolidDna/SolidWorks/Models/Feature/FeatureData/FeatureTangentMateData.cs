using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Tangent Mate feature data
    /// </summary>
    public class FeatureTangentMateData : SolidDnaObject<ITangentMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureTangentMateData(ITangentMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}