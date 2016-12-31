using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Simulation Linear Spring feature data
    /// </summary>
    public class FeatureSimulationLinearSpringData : SolidDnaObject<ISimulationLinearSpringFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSimulationLinearSpringData(ISimulationLinearSpringFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
