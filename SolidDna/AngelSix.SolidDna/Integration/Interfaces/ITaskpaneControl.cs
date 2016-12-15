namespace AngelSix.SolidDna
{
    /// <summary>
    /// A user control that is to be used as a UI control inside the SolidWorks Taskpane
    /// </summary>
    public interface ITaskpaneControl
    {
        /// <summary>
        /// The unique ProgId of this control
        /// </summary>
        string ProgId { get; }
    }
}
