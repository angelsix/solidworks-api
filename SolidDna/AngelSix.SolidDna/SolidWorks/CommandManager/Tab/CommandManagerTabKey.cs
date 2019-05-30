namespace AngelSix.SolidDna
{
    /// <summary>
    /// A key to uniquely identify a command tab
    /// </summary>
    public class CommandManagerTabKey
    {
        /// <summary>
        /// The title of the tab
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The model type for this tab 
        /// </summary>
        public ModelType ModelType { get; set; }
    }
}
