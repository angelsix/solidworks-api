using SolidWorks.Interop.swconst;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// The options from a <see cref="Model.PackAndGo(string)"/> call,
    /// from <see cref="swPackAndGoSaveStatus_e"/>
    /// </summary>
    public enum PackAndGoSaveStatus
    {
        /// <summary>
        /// Successfully saved
        /// </summary>
        Success = 0,

        /// <summary>
        /// User input was not correct
        /// </summary>
        UserInputNotCorrect = 1,

        /// <summary>
        /// File already exists
        /// </summary>
        FileAlreadyExists = 2,

        /// <summary>
        /// Saving an empty file
        /// </summary>
        SavingEmptyFile = 3,

        /// <summary>
        /// Error when saving
        /// </summary>
        SaveError = 4
    }
}
