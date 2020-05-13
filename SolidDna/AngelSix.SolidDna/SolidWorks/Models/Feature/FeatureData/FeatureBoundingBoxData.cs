using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Bounding Box feature data
    /// </summary>
    public class FeatureBoundingBoxData : SolidDnaObject<IBoundingBoxFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureBoundingBoxData(IBoundingBoxFeatureData model) : base(model)
        {

        }

        #endregion
    }
}