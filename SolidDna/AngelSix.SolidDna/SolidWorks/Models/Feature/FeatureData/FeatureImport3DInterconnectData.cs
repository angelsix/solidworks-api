using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Import 3D Interconnect feature data
    /// </summary>
    public class FeatureImport3DInterconnectData : SolidDnaObject<IImport3DInterconnectData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureImport3DInterconnectData(IImport3DInterconnectData model) : base(model)
        {

        }

        #endregion
    }
}