using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Reference Point feature data
    /// </summary>
    public class FeatureReferencePointData : SolidDnaObject<IRefPointFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureReferencePointData(IRefPointFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
