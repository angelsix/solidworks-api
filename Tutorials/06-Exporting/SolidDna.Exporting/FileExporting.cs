using AngelSix.SolidDna;
using Microsoft.Win32;
using SolidWorks.Interop.swconst;
using System;

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
            if (Dna.Application.ActiveModel?.IsPart != true)
            {
                // Tell user
                Dna.Application.ShowMessageBox("Active model is not a part", SolidWorksMessageBoxIcon.Stop);

                return;
            }

            // Ask the user where to export the file
            var location = GetSaveLocation("DXF Flat Pattern|*.dxf", "Save Part as DXF");

            // If the user cancelled, return
            if (string.IsNullOrEmpty(location))
                return;

            // Now export as DXF
            Dna.Application.ActiveModel.AsPart().ExportFlatPatternView(location, (int)swExportFlatPatternViewOptions_e.swExportFlatPatternOption_RemoveBends);

            // Tell user
            Dna.Application.ShowMessageBox("Successfully saved part as DXF");
        }

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
    }
}
