using SolidWorks.Interop.sldworks;
using System.Linq;

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

        #region View Methods

        /// <summary>
        /// Activates the specified drawing view
        /// </summary>
        /// <param name="viewName">Name of the drawing view</param>
        /// <returns>True if successful, false if not</returns>
        public bool ActivateView(string viewName) => mBaseObject.ActivateView(viewName);

        /// <summary>
        /// Rotates the view so the selected line in the view is horizontal
        /// </summary>
        public void AlignViewHorizontally() => mBaseObject.AlignHorz();

        /// <summary>
        /// Rotates the view so the selected line in the view is vertical
        /// </summary>
        public void AlignViewVertically() => mBaseObject.AlignVert();

        #endregion

        #region Dimension Methods

        /// <summary>
        /// Adds a chamfer dimension to the selected edges
        /// </summary>
        /// <param name="x">X dimension</param>
        /// <param name="y">Y dimension</param>
        /// <param name="z">Z dimension</param>
        /// <returns>The chamfer <see cref="ModelDisplayDimension"/> if successful. Null if not.</returns>
        /// <remarks>Make sure to select the 2 edges of the chamfer before running this command</remarks>
        public ModelDisplayDimension AddChamferDimension(double x, double y, double z)
            => new ModelDisplayDimension((IDisplayDimension)mBaseObject.AddChamferDim(x, y, z)).CreateOrNull();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">X dimension</param>
        /// <param name="y">Y dimension</param>
        /// <param name="z">Z dimension</param>
        /// <returns>The hole cutout <see cref="ModelDisplayDimension"/> if successful. Null if not.</returns>
        /// <remarks>Make sure to select the hole sketch circle before running this command</remarks>
        public ModelDisplayDimension AddHoleCutout(double x, double y, double z)
            => new ModelDisplayDimension((IDisplayDimension)mBaseObject.AddHoleCallout2(x, y, z)).CreateOrNull();

        /// <summary>
        /// Re-aligns the selected ordinate dimension if it was previously broken
        /// </summary>
        public void AlignOrdinateDimension() => mBaseObject.AlignOrdinate();

        #endregion

        #region Annotation Methods

        /// <summary>
        /// Attaches an existing annotation to a drawing sheet or view
        /// </summary>
        /// <param name="option">The attach option</param>
        /// <returns>True if successful, false if not</returns>
        /// <remarks>
        ///     Remember to select the annotation and if attaching to a view select an
        ///     element on the view also before running this command
        /// </remarks>
        public bool AttachAnnotation(AttachAnnotationOption option) => mBaseObject.AttachAnnotation((int)option);

        /// <summary>
        /// Attempts to attach unattached dimensions, for example in an imported DXF file
        /// </summary>
        public void AttachDimensions() => mBaseObject.AttachDimensions();

        #endregion

        #region Line Style Methods

        /// <summary>
        /// Adds a line style to the drawing document
        /// </summary>
        /// <param name="styleName">The name of the style</param>
        /// <param name="boldLineEnds">True to have bold dots at each end of the line</param>
        /// <param name="segments">Segments. Positive numbers are dashes, negative are gaps</param>
        /// <returns></returns>
        /// <example>
        ///     AddLineStyle("NewStyle", true, 1.25,-0.5,0.5,-0.5);
        ///     To add a new line like this:
        ///     -----  --  -----  --  -----  --
        /// </example>
        public bool AddLineStyle(string styleName, bool boldLineEnds, params double[] segments)
        {
            // Set line end style
            var segmentString = boldLineEnds ? "B," : "A,";

            // Add segments
            segmentString += string.Join(",", segments.Select(f => f.ToString()));

            // Add line style
            return mBaseObject.AddLineStyle(styleName, segmentString);
        }

        #endregion
    }
}
