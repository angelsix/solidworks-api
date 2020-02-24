using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
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

        #region Public Properties

        /// <summary>
        /// The raw underlying COM object
        /// WARNING: Use with caution. You must handle all disposal from this point on
        /// </summary>
        public DrawingDoc UnsafeObject => mBaseObject;

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
        /// Gets the name of the currently active sheet
        /// </summary>
        /// <returns></returns>
        public string CurrentActiveSheet()
        {
            using (var sheet = new DrawingSheet((Sheet)mBaseObject.GetCurrentSheet(), this))
            {
                return sheet.SheetName;
            }
        }

        /// <summary>
        /// Gets the sheet names of the drawing
        /// </summary>
        /// <returns></returns>
        public string[] SheetNames() => (string[])mBaseObject.GetSheetNames();
        
        public void ForEachSheet(Action<DrawingSheet> sheetsCallback)
        {
            // Get each sheet name
            var sheetNames = SheetNames();

            // Get all sheet names
            foreach (var sheetName in sheetNames)
            {
                // Get instance of sheet
                using (var sheet = new DrawingSheet(mBaseObject.Sheet[sheetName], this))
                {
                    // Callback
                    sheetsCallback(sheet);
                }
            }
        }

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

        /// <summary>
        /// Gets all the views of the drawing
        /// </summary>
        /// <param name="viewsCallback">The callback containing all views</param>
        public void Views(Action<List<DrawingView>> viewsCallback)
        {
            // List of all views
            var views = new List<DrawingView>();

            // Get all views as an array of arrays
            var sheetArray = (object[])mBaseObject.GetViews();

            // Get all views
            foreach (object[] viewArray in sheetArray)
                foreach (View view in viewArray)
                    views.Add(new DrawingView((View)view));

            try
            {
                // Callback
                viewsCallback(views);
            }
            finally
            {
                // Dispose all views
                views.ForEach(view => view.Dispose());
            }
        }

        #endregion

        #region Balloon Methods

        /// <summary>
        /// Automatically inserts BOM balloons in selected drawing views
        /// </summary>
        /// <param name="options">The balloon options</param>
        /// <param name="onSuccess">Callback containing all created notes if successful</param>
        /// <returns>An array of the created <see cref="Note"/> objects</returns>
        /// <remarks>
        ///     This method automatically inserts BOM balloons for bill of materials 
        ///     items in selected drawing views. If a drawing sheet is selected, BOM 
        ///     balloons are automatically inserted for all of the drawing views on that 
        ///     drawing sheet. 
        ///     
        ///     To automatically insert BOM balloons, select one or more views or sheets
        ///     for which to automatically create BOM balloons, then call this method.
        /// </remarks>
        public void AutoBalloon(AutoBalloonOptions options, Action<Note[]> onSuccess = null)
        {
            // Create native options
            var nativeOptions = mBaseObject.CreateAutoBalloonOptions();

            // Fill options
            nativeOptions.CustomSize = options.CustomSize;
            nativeOptions.EditBalloonOption = (int)options.EditBalloonOptions;
            nativeOptions.EditBalloons = options.EditBalloons;
            nativeOptions.FirstItem = options.FirstItem;
            nativeOptions.IgnoreMultiple = options.IgnoreMultiple;
            nativeOptions.InsertMagneticLine = options.InsertMagnaticLine;
            nativeOptions.ItemNumberIncrement = options.ItemNumberIncrement;
            nativeOptions.ItemNumberStart = options.ItemNumberStart;
            nativeOptions.ItemOrder = (int)options.ItemOrder;
            nativeOptions.Layername = options.LayerName;
            nativeOptions.Layout = (int)options.Layout;
            nativeOptions.LeaderAttachmentToFaces = options.LeaderAttachmentToFaces;
            nativeOptions.LowerText = options.LowerText;
            nativeOptions.LowerTextContent = (int)options.LowerTextContent;
            nativeOptions.ReverseDirection = options.ReverseDirection;
            nativeOptions.Size = (int)options.Size;
            nativeOptions.Style = (int)options.Style;
            nativeOptions.UpperText = options.UpperText;
            nativeOptions.UpperTextContent = (int)options.UpperTextContent;

            // Create the notes
            var nativeNotes = (object[])mBaseObject.AutoBalloon5(nativeOptions);

            // If we have a callback, and have notes...
            if (onSuccess != null && nativeNotes?.Length > 0)
            {
                // Create all note classes
                var notes = nativeNotes.Select(f => new Note((INote)f)).ToArray();

                // Inform listeners
                onSuccess.Invoke(notes);

                // Dispose them
                foreach (var note in notes)
                    note.Dispose();
            }
        }

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
