using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Mirror Part feature data
    /// </summary>
    public class FeatureMirrorPartData : SolidDnaObject<IMirrorPartFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMirrorPartData(IMirrorPartFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
