using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Angle Mate feature data
    /// </summary>
    public class FeatureAngleMateData : SolidDnaObject<IAngleMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureAngleMateData(IAngleMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}