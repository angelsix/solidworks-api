using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Simulation Motor feature data
    /// </summary>
    public class FeatureSimulationMotorData : SolidDnaObject<ISimulationMotorFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSimulationMotorData(ISimulationMotorFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
