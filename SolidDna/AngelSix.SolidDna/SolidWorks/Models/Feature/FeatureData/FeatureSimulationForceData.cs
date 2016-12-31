using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Simulation Force feature data
    /// </summary>
    public class FeatureSimulationForceData : SolidDnaObject<ISimulationForceFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSimulationForceData(ISimulationForceFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
