using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Parting Line feature data
    /// </summary>
    public class FeaturePartingLineData : SolidDnaObject<IPartingLineFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeaturePartingLineData(IPartingLineFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
