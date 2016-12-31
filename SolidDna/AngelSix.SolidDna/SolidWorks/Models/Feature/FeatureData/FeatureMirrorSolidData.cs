using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Mirror Solid feature data
    /// </summary>
    public class FeatureMirrorSolidData : SolidDnaObject<IMirrorSolidFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMirrorSolidData(IMirrorSolidFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
