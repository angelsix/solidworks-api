using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Simulation Damper feature data
    /// </summary>
    public class FeatureSimulationDamperData : SolidDnaObject<ISimulationDamperFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSimulationDamperData(ISimulationDamperFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
