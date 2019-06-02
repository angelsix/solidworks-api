using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Exposes all Drawing Document calls from a <see cref="Model"/>
    /// </summary>
    public class DrawingDocument
    {
        #region Protected Members

        /// <summary>
        /// The base model document. Note we do not dispose of this (the parent Model will)
        /// </summary>
        protected DrawingDoc mBaseObject;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DrawingDocument(DrawingDoc model)
        {
            mBaseObject = model;
        }

        #endregion

        #region Sheet Methods

        /// <summary>
        /// Activates the specified drawing sheet
        /// </summary>
        /// <param name="sheetName">Name of the sheet</param>
        /// <returns>True if the sheet was activated, false if SOLIDWORKS generated an error</returns>
        public bool ActivateSheet(string sheetName) => mBaseObject.ActivateSheet(sheetName);

        /// <summary>
        /// Activates the specified drawing view
        /// </summary>
        /// <param name="viewName">Name of the drawing view</param>
        /// <returns>True if successful, false if not</returns>
        public bool ActivateView(string viewName) => mBaseObject.ActivateView(viewName);

        /// <summary>
        /// Adds a chamfer dimension to the selected edges
        /// </summary>
        /// <param name="x">X dimension</param>
        /// <param name="y">Y dimension</param>
        /// <param name="z">Z dimension</param>
        /// <returns>The chamfer <see cref="ModelDisplayDimension"/> if successful. Null if not.</returns>
        /// <remarks>Make sure to select the 2 edges of the chamfer before adding.</remarks>
        public ModelDisplayDimension AddChamferDimension(double x, double y, double z)
        {
            var dimension = (IDisplayDimension)mBaseObject.AddChamferDim(x, y, z);

            return dimension != null ? new ModelDisplayDimension(dimension) : null;
        }

        #endregion
    }
}
