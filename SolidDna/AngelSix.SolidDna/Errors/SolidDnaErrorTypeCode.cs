namespace AngelSix.SolidDna
{
    /// <summary>
    /// A list of all known types of error type codes in SolidDNA
    /// </summary>
    public enum SolidDnaErrorTypeCode
    {
        /// <summary>
        /// The error type is not known and likely was not caught anywhere expected
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The error was caught but was not an expected type of error
        /// </summary>
        Unexpected = 1,

        /// <summary>
        /// An error occurred while working with a file on the file system
        /// </summary>
        File = 2,

        /// <summary>
        /// An error occurred trying to perform a SolidWorks API call on the Taskpane
        /// </summary>
        SolidWorksTaskpane = 10,

        /// <summary>
        /// An error occurred trying to perform a top level SolidWorks API call
        /// </summary>
        SolidWorksApplication = 11,

        /// <summary>
        /// An error occurred trying to perform a SolidWorks API call on a Model
        /// </summary>
        SolidWorksModel = 12,

        /// <summary>
        /// An error occurred trying to perform a SolidWorks API call on the Command Manager
        /// </summary>
        SolidWorksCommandManager = 13,

        /// <summary>
        /// An error occurred trying to perform a SolidWorks API call on export data
        /// </summary>
        ExportData = 14,

    }
}
