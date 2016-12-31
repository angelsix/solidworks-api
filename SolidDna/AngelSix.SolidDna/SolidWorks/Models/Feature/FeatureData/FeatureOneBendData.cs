using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks One Bend feature data
    /// </summary>
    public class FeatureOneBendData : SolidDnaObject<IOneBendFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureOneBendData(IOneBendFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
