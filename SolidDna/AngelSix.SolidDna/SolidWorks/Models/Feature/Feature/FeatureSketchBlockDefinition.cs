using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Sketch Block Definition feature
    /// </summary>
    public class FeatureSketchBlockDefinition : SolidDnaObject<ISketchBlockDefinition>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeatureSketchBlockDefinition(ISketchBlockDefinition model) : base(model)
        {

        }

        #endregion
    }
}
