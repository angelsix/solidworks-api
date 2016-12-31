using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Derived Part feature data
    /// </summary>
    public class FeatureDerivedPartData : SolidDnaObject<IDerivedPartFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureDerivedPartData(IDerivedPartFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
