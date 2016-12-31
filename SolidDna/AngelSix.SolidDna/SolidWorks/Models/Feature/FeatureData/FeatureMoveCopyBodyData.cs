using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Move Copy Body feature data
    /// </summary>
    public class FeatureMoveCopyBodyData : SolidDnaObject<IMoveCopyBodyFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMoveCopyBodyData(IMoveCopyBodyFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
