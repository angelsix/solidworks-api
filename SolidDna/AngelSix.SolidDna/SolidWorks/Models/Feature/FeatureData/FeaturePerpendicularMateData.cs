using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Perpendicular Mate feature data
    /// </summary>
    public class FeaturePerpendicularMateData : SolidDnaObject<IPerpendicularMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeaturePerpendicularMateData(IPerpendicularMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}