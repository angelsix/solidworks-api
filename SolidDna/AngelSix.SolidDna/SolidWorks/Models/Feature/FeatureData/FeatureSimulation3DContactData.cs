using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Simulation 3D Contact feature data
    /// </summary>
    public class FeatureSimulation3DContactData : SolidDnaObject<ISimulation3DContactFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSimulation3DContactData(ISimulation3DContactFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
