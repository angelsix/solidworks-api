using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Delete Face feature data
    /// </summary>
    public class FeatureDeleteFaceData : SolidDnaObject<IDeleteFaceFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureDeleteFaceData(IDeleteFaceFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
