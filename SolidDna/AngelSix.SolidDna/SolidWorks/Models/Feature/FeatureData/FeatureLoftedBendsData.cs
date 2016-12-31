using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Lofted Bends feature data
    /// </summary>
    public class FeatureLoftedBendsData : SolidDnaObject<ILoftedBendsFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureLoftedBendsData(ILoftedBendsFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
