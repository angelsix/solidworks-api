using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Simulation Gravity feature data
    /// </summary>
    public class FeatureSimulationGravityData : SolidDnaObject<ISimulationGravityFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSimulationGravityData(ISimulationGravityFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
