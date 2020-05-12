using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Mirror Component feature data
    /// </summary>
    public class FeatureMirrorComponentData : SolidDnaObject<IMirrorComponentFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMirrorComponentData(IMirrorComponentFeatureData model) : base(model)
        {

        }

        #endregion
    }
}