using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// The data used to specify options when exporting as a PDF
    /// </summary>
    public class PdfExportData
    {
        #region Protected Members

        /// <summary>
        /// The native export data object
        /// </summary>
        protected IExportPdfData mExportData;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the underlying export data needed for the underlying ModelExtension SaveAs call
        /// </summary>
        public IExportPdfData ExportData => mExportData;

        /// <summary>
        /// If true the PDF will be opened after it has been saved
        /// </summary>
        public bool ViewPdfAfterSaving
        {
            get => mExportData.ViewPdfAfterSaving;
            set => mExportData.ViewPdfAfterSaving = value;
        }

        /// <summary>
        /// Specifies if the PDF output should be in 3D PDF format
        /// </summary>
        public bool ExportAs3D
        {
            get => mExportData.ExportAs3D;
            set => mExportData.ExportAs3D = value;
        }

        /// <summary>
        /// Gets the current setting (set using <see cref="SetSheets(PdfSheetsToExport, List{string})"/>)
        /// for which sheets are to be exported.
        /// </summary>
        /// <remarks>
        /// To set the value, use <see cref="SetSheets(PdfSheetsToExport, List{string})"/>
        /// </remarks>
        public PdfSheetsToExport WhichSheets => (PdfSheetsToExport)mExportData.GetWhichSheets();

        /// <summary>
        /// Gets the specific sheets names that will be exported.
        /// </summary>
        /// <remarks>
        /// To set the value, use <see cref="SetSheets(PdfSheetsToExport, List{string})"/>
        /// </remarks>
        public string[] Sheets => mExportData.GetSheets() as string[];

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PdfExportData()
        {
            // Wrap any error
            SolidDnaErrors.Wrap(() =>
            {
                // Try and get export data object
                mExportData = SolidWorksEnvironment.Application.GetPdfExportData();
            },
                SolidDnaErrorTypeCode.ExportData,
                SolidDnaErrorCode.SolidWorksExportDataGetPdfExportDataError,
                Localization.GetString("SolidWorksExportDataGetPdfExportDataError"));
        }

        #endregion

        #region PDF Export Methods

        /// <summary>
        /// Sets which sheets should be exported when saving
        /// </summary>
        /// <param name="whichSheets">Which sheets to be exported</param>
        /// <param name="specificSheets">If specific sheets was specified, this the list of named sheets to be exported</param>
        /// <returns>Returns true if successful</returns>
        public bool SetSheets(PdfSheetsToExport whichSheets, List<string> specificSheets)
        {
            // Wrap any error
            return SolidDnaErrors.Wrap(() =>
            {
                // Try and set sheets
                return mExportData.SetSheets((int)whichSheets, specificSheets.ToArray());
            },
                SolidDnaErrorTypeCode.ExportData,
                SolidDnaErrorCode.SolidWorksExportDataPdfSetSheetsError,
                Localization.GetString("SolidWorksExportDataPdfSetSheetsError"));
        }

        #endregion
    }
}
