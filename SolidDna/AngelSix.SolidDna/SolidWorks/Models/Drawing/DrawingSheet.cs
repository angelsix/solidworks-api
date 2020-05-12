using SolidWorks.Interop.sldworks;
using System.Drawing;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A sheet of a drawing
    /// </summary>
    public class DrawingSheet: SolidDnaObject<Sheet>
    {
        #region Private Members

        /// <summary>
        /// The parent drawing document of this sheet
        /// </summary>
        private readonly DrawingDocument mDrawingDoc;

        #endregion

        #region Public Properties

        /// <summary>
        /// The sheet name
        /// </summary>
        public string SheetName => BaseObject.GetName();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="comObject">The underlying COM object</param>
        /// <param name="drawing">The parent drawing document</param>
        public DrawingSheet(Sheet comObject, DrawingDocument drawing) : base(comObject)
        {
            mDrawingDoc = drawing;
        }

        #endregion

        /// <summary>
        /// Activates this sheet
        /// </summary>
        public bool Activate() => mDrawingDoc.ActivateSheet(SheetName);
    }
}
