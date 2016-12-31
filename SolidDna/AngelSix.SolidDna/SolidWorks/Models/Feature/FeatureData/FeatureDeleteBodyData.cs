using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Delete Body feature data
    /// </summary>
    public class FeatureDeleteBodyData : SolidDnaObject<IDeleteBodyFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureDeleteBodyData(IDeleteBodyFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
