namespace AngelSix.SolidDna
{
    /// <summary>
    /// A custom property of a model that can be edit directly
    /// </summary>
    public class CustomProperty
    {
        #region Private Members

        /// <summary>
        /// The editor used for this custom property
        /// </summary>
        private CustomPropertyEditor mEditor;

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the custom property
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The value of the custom property
        /// </summary>
        public string Value
        {
            get => mEditor.GetCustomProperty(Name);
            set => mEditor.SetCustomProperty(Name, value);
        }

        /// <summary>
        /// The resolved value of the custom property
        /// </summary>
        public string ResolvedValue => mEditor.GetCustomProperty(Name, resolve: true);

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CustomProperty(CustomPropertyEditor editor, string name)
        {
            // Store reference
            mEditor = editor;

            // Set name
            Name = name;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deletes this custom property
        /// </summary>
        public void Delete()
        {
            mEditor.DeleteCustomProperty(Name);
        }

        #endregion
    }
}
