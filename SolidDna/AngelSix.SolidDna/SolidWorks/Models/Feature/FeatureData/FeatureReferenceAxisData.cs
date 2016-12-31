using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Reference Axis feature data
    /// </summary>
    public class FeatureReferenceAxisData : SolidDnaObject<IRefAxisFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureReferenceAxisData(IRefAxisFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
