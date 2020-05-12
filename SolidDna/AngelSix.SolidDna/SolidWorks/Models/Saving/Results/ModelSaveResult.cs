namespace AngelSix.SolidDna
{
    /// <summary>
    /// The result of a file save attempt
    /// </summary>
    public class ModelSaveResult
    {
        /// <summary>
        /// Whether the save operation was successful or not
        /// </summary>
        public bool Successful => Errors == 0;

        /// <summary>
        /// Any warnings for the file save operation
        /// </summary>
        public SaveAsWarnings Warnings { get; set; }

        /// <summary>
        /// Any errors for the file save operation
        /// </summary>
        public SaveAsErrors Errors { get; set; }

        /// <summary>
        /// Nice output to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Successful ? "Success" : $"Failed. Errors ({Errors}). Warnings ({Warnings})";
        }
    }
}
