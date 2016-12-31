
namespace AngelSix.SolidDna
{
    /// <summary>
    /// A list of all known types of error codes in SolidDNA
    /// </summary>
    public enum SolidDnaErrorCode
    {
        #region File (1,000)

        /// <summary>
        /// There was an unknown error while accessing a file
        /// </summary>
        FileUnexpectedError = 1000,

        #endregion

        #region SolidWorks Application (9,000)

        /// <summary>
        /// There was an unknown error while running a top level SolidWorks API call on the application
        /// </summary>
        SolidWorksApplicationError = 9000,

        /// <summary>
        /// There was an error in the event for when a new file is about to be opened
        /// </summary>
        SolidWorksApplicationFilePreOpenError = 9002,

        /// <summary>
        /// There was an error in the event for when a new file has finished opening
        /// </summary>
        SolidWorksApplicationFilePostOpenError = 9002,

        /// <summary>
        /// There was an error in the event for when the active model has changed
        /// </summary>
        SolidWorksApplicationActiveModelChangedError = 9003,

        /// <summary>
        /// There was an error when trying to read the materials databases
        /// </summary>
        SolidWorksApplicationGetMaterialsError = 9004,

        /// <summary>
        /// There was an error when trying to read the materials databases because the database file does not exist
        /// </summary>
        SolidWorksApplicationGetMaterialsFileNotFoundError = 9005,

        /// <summary>
        /// There was an error when trying to read the materials databases because the database file was of an unexpected format
        /// </summary>
        SolidWorksApplicationGetMaterialsFileFormatError = 9006,

        /// <summary>
        /// There was an error when trying to find a material in a materials database file
        /// </summary>
        SolidWorksApplicationFindMaterialsError = 9007,

        /// <summary>
        /// There was an error when trying to get the SolidWorks version number
        /// </summary>
        SolidWorksApplicationVersionError = 9008,

        #endregion

        #region SolidWorks Taskpane (10,000)

        /// <summary>
        /// There was an unknown error while running a SolidWorks API call related to the Taskpane
        /// </summary>
        SolidWorksTaskpaneError = 10000,

        /// <summary>
        /// There was an error while running a SolidWorks API call to create a Taskpane
        /// </summary>
        SolidWorksTaskpaneCreateError = 10001,

        /// <summary>
        /// There was an error while running a SolidWorks API call to add a control to a Taskpane
        /// </summary>
        SolidWorksTaskpaneAddControlError = 10002,

        #endregion

        #region SolidWorks Model (11,000)

        /// <summary>
        /// There was an unknown error while running a SolidWorks API call on a SolidWorks model
        /// </summary>
        SolidWorksModelError = 11000,

        /// <summary>
        /// There was an error while trying to get the material on a SolidWorks model
        /// </summary>
        SolidWorksModelGetMaterialError = 11001,

        /// <summary>
        /// There was an error while trying to set the material on a SolidWorks model
        /// </summary>
        SolidWorksModelSetMaterialError = 11002,

        /// <summary>
        /// There was an error while trying to get the mass on a SolidWorks model
        /// </summary>
        SolidWorksModelGetMassPropertiesError = 11003,

        /// <summary>
        /// There was an error casting a selected object to a specific type
        /// </summary>
        SolidWorksModelSelectedObjectCastError = 11004,

        #endregion

    }
}
