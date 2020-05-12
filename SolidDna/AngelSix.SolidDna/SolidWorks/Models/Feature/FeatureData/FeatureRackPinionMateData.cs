using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Rack Pinion Mate feature data
    /// </summary>
    public class FeatureRackPinionMateData : SolidDnaObject<IRackPinionMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureRackPinionMateData(IRackPinionMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}