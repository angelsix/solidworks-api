using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Slot Mate feature data
    /// </summary>
    public class FeatureSlotMateData : SolidDnaObject<ISlotMateFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSlotMateData(ISlotMateFeatureData model) : base(model)
        {

        }

        #endregion
    }
}