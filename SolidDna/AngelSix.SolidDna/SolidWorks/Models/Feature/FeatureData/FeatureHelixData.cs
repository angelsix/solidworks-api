using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Helix feature data
    /// </summary>
    public class FeatureHelixData : SolidDnaObject<IHelixFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureHelixData(IHelixFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
