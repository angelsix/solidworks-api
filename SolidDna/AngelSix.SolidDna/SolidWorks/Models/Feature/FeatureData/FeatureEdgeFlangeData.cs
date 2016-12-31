using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Edge Flange feature data
    /// </summary>
    public class FeatureEdgeFlangeData : SolidDnaObject<IEdgeFlangeFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureEdgeFlangeData(IEdgeFlangeFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
