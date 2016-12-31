using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Miter Flange feature data
    /// </summary>
    public class FeatureMiterFlangeData : SolidDnaObject<IMiterFlangeFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureMiterFlangeData(IMiterFlangeFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
