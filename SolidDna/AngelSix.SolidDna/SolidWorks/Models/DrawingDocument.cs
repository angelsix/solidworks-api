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

        #endregion
    }
}
