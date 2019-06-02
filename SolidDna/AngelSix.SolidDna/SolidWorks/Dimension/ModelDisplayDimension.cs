using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks Display Dimension
    /// </summary>
    public class ModelDisplayDimension : SolidDnaObject<IDisplayDimension>
    {
        #region Public Properties

        /// <summary>
        /// The selection name for this dimension that can be used to select it.
        /// For example D1@Sketch1
        /// </summary>
        public string SelectionName => BaseObject.GetNameForSelection();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ModelDisplayDimension(IDisplayDimension model) : base(model)
        {
            
        }

        #endregion
    }
}
