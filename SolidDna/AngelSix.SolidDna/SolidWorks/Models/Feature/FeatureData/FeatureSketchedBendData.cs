using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sketched Bend feature data
    /// </summary>
    public class FeatureSketchedBendData : SolidDnaObject<ISketchedBendFeatureData>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSketchedBendData(ISketchedBendFeatureData model) : base(model)
        {

        }

        #endregion
    }
}
