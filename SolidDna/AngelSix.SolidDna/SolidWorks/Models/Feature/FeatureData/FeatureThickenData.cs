using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Thicken feature data
    /// </summary>
    public class FeatureThickenData : SolidDnaObject<IThickenFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureThickenData(IThickenFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
