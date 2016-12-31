using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Save Body feature data
    /// </summary>
    public class FeatureSaveBodyData : SolidDnaObject<ISaveBodyFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSaveBodyData(ISaveBodyFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
