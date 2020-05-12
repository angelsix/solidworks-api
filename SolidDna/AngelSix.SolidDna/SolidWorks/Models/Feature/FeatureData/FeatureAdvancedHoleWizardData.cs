using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Advanced Hole Wizard feature data
    /// </summary>
    public class FeatureAdvancedHoleWizardData : SolidDnaObject<IAdvancedHoleFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureAdvancedHoleWizardData(IAdvancedHoleFeatureData model) : base(model)
        {

        }

        #endregion
    }
}