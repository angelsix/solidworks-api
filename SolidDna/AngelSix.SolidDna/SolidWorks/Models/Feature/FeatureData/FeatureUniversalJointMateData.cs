using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Universal Joint Mate feature data
    /// </summary>
    public class FeatureUniversalJointMateData : SolidDnaObject<IUniversalJointMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureUniversalJointMateData(IUniversalJointMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}