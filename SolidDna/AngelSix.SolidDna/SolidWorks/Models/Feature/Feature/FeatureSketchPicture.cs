using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sketch Picture feature
    /// </summary>
    public class FeatureSketchPicture : SolidDnaObject<ISketchPicture>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSketchPicture(ISketchPicture model) : base(model)
        {

        }

        #endregion
    }
}
