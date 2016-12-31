using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Library Feature feature data
    /// </summary>
    public class FeatureLibraryFeatureData : SolidDnaObject<ILibraryFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureLibraryFeatureData(ILibraryFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
