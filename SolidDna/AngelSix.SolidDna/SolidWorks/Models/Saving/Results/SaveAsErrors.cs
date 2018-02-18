using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Any errors of a model save operation. 
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Errors are bit-masked (flags) so you can get each error via:
    ///     
    ///     <code>
    ///         errors.GetFlags();
    ///     </code>
    /// </para>
    /// </remarks>
    [Flags]
    public enum SaveAsErrors
    {
        /// <summary>
        /// No errors
        /// </summary>
        None = 0,

        /// <summary>
        /// There was an unknown error
        /// </summary>
        GenericSaveError = 1,

        /// <summary>
        /// Failed to save as the destination file is read-only
        /// </summary>
        ReadOnlySaveError = 2,

        /// <summary>
        /// The filename was not provided
        /// </summary>
        FileNameEmpty = 4,

        /// <summary>
        /// Filename cannot contain the @ symbol
        /// </summary>
        FileNameContainsAtSign = 8,

        /// <summary>
        /// The file is write-locked
        /// </summary>
        FileLockError = 16,

        /// <summary>
        /// The save as file type is not valid
        /// </summary>
        FileSaveFormatNotAvailable = 32,

        /// <summary>
        /// Obsolete: This error is now in a warning
        /// </summary>
        [Obsolete("This error is now a warning")]
        FileSaveWithRebuildError = 64,

        /// <summary>
        /// The file already exists, and the save is set to not override existing files
        /// </summary>
        FileSaveAsDoNotOverwrite = 128,

        /// <summary>
        /// The file save extension does not match the SolidWorks document type
        /// </summary>
        FileSaveAsInvalidFileExtension = 256,

        /// <summary>
        /// Save the selected bodies in a part document. Valid option for IPartDoc::SaveToFile2; 
        /// however, not a valid option for IModelDocExtension::SaveAs
        /// </summary>
        FileSaveAsNoSelection = 512,

        /// <summary>
        /// The version of eDrawings is invalid
        /// </summary>
        FileSaveAsBadEDrawingsVersion = 1024,

        /// <summary>
        /// The filename is too long
        /// </summary>
        FileSaveAsNameExceedsMaxPathLength = 2048,

        /// <summary>
        /// The save as operation is not supported, or was executed is such a way that the resulting
        /// file might not be complete, possibly because SolidWorks is hidden; 
        /// </summary>
        FileSaveAsNotSupported = 4096,

        /// <summary>
        /// Saving an assembly with renamed components requires saving the references
        /// </summary>
        FileSaveRequiresSavingReferences = 8192
    }
}
