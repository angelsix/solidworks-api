using AngelSix.SolidDna;
using Microsoft.Win32;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using static AngelSix.SolidDna.SolidWorksEnvironment;

namespace SolidDna.Exporting
{
    /// <summary>
    /// Helper functions for exporting files in different formats
    /// </summary>
    public static class FileExporting
    {
        /// <summary>
        /// Exports the currently active part as a DXF
        /// </summary>
        public static void ExportPartAsDxf()
        {
            // Make sure we have a part
            if (Application.ActiveModel?.IsPart != true)
            {
                // Tell user
                Application.ShowMessageBox("Active model is not a part", SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Ask the user where to export the file
            var location = GetSaveLocation("DXF Flat Pattern|*.dxf", "Save Part as DXF");

            // If the user canceled, return
            if (string.IsNullOrEmpty(location))
                return;

            // Now export as DXF
            Application.ActiveModel.AsPart().ExportFlatPatternView(location, (int)swExportFlatPatternViewOptions_e.swExportFlatPatternOption_RemoveBends);

            // Tell user
            Application.ShowMessageBox("Successfully saved part as DXF");
        }

        /// <summary>
        /// Exports the currently active part as a STEP
        /// </summary>
        public static void ExportModelAsStep()
        {
            // Make sure we have a part or assembly
            if (Application.ActiveModel?.IsPart != true && Application.ActiveModel?.IsAssembly != true)
            {
                // Tell user
                Application.ShowMessageBox("Active model is not a part or assembly", SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Ask the user where to export the file
            var location = GetSaveLocation("STEP File|*.step", "Save Part as STEP");

            // If the user canceled, return
            if (string.IsNullOrEmpty(location))
                return;

            // Try and save the file...
            var result = Application.ActiveModel.SaveAs(
                // Save as location
                location,
                // Do it silently and as a copy, updating anything thats needed before saving
                options: SaveAsOptions.Silent | SaveAsOptions.Copy | SaveAsOptions.UpdateInactiveViews);


            if (!result.Successful)
                // Tell user failed
                Application.ShowMessageBox("Failed to save model as STEP");
            else
                // Tell user success
                Application.ShowMessageBox("Successfully saved model as STEP");
        }

        /// <summary>
        /// Exports the currently active part as a PDF
        /// </summary>
        public static void ExportDrawingAsPdf()
        {
            // Make sure we have a part or assembly
            if (Application.ActiveModel?.IsDrawing != true)
            {
                // Tell user
                Application.ShowMessageBox("Active model is not a drawing", SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Ask the user where to export the file
            var location = GetSaveLocation("PDF File|*.pdf", "Save Part as PDF");

            // If the user canceled, return
            if (string.IsNullOrEmpty(location))
                return;

            // Get sheet names
            var sheetNames = new List<string>((string[])Application.ActiveModel.AsDrawing().GetSheetNames());

            // Set PDF sheet settings
            var exportData = new PdfExportData();
            exportData.SetSheets(PdfSheetsToExport.ExportSpecifiedSheets, sheetNames);

            // Try and save the file...
            var result = Application.ActiveModel.SaveAs(
                // Save as location
                location,
                // Do it silently and as a copy, updating anything thats needed before saving
                options: SaveAsOptions.Silent | SaveAsOptions.Copy | SaveAsOptions.UpdateInactiveViews,
                // Pass in PDF export data
                pdfExportData: exportData);

            if (!result.Successful)
                // Tell user failed
                Application.ShowMessageBox("Failed to save drawing as PDF");
            else
                // Tell user success
                Application.ShowMessageBox("Successfully saved drawing as PDF");
        }

        #region Private Helpers
        
        /// <summary>
        /// Asks the user for a save filename and location
        /// </summary>
        /// <param name="filter">The filter for the save dialog box</param>
        /// <param name="title">The title of the dialog box</param>
        /// <returns></returns>
        private static string GetSaveLocation(string filter, string title)
        {
            // Create dialog
            var dialog = new SaveFileDialog { Filter = filter, Title = title, AddExtension = true };

            // Get dialog result
            if (dialog.ShowDialog() == true)
                return dialog.FileName;
            else
                return null;
        }

        #endregion
    }
}
