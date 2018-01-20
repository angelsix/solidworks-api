namespace AngelSix.SolidDna
{
    /// <summary>
    /// Specifies which drawing sheets will get exported to PDF, from <see cref="SolidWorks.Interop.swconst.swExportDataSheetsToExport_e"/>
    /// </summary>
    public enum PdfSheetsToExport
    {
        /// <summary>
        /// Exports all drawing sheets
        /// </summary>
        ExportAllSheets = 1,

        /// <summary>
        /// Exports the currently active sheet
        /// </summary>
        ExportCurrentSheet = 2,

        /// <summary>
        /// Exports the sheets specified in the sheets array
        /// </summary>
        ExportSpecifiedSheets = 3
    }
}
