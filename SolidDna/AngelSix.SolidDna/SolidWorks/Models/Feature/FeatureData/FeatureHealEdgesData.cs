using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Heal Edges feature data
    /// </summary>
    public class FeatureHealEdgesData : SolidDnaObject<IHealEdgesFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureHealEdgesData(IHealEdgesFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
