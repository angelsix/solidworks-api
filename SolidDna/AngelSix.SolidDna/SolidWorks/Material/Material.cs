namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents material information for a SolidWorks material
    /// </summary>
    public class Material
    {
        /// <summary>
        /// The classification this material belongs to (like a group). 
        /// For example Building Materials, Steel, Wood etc...
        /// </summary>
        public string Classification { get; set; }

        /// <summary>
        /// The name of the material
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description for this material
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The full path of the database that contains this material
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// The name and classification of the material
        /// </summary>
        public string DisplayName => $"{Name} ({Classification})";

        /// <summary>
        /// Indicates if this material information was found in a known database file
        /// 
        /// If false, then the <see cref="Database"/> will be just a filename with no extension
        /// of the missing database file.
        /// 
        /// If true then <see cref="Database"/> will be the full file path to the database file
        /// </summary>
        public bool DatabaseFileFound { get; set; }

        /// <summary>
        /// Show nice string for debugging purposes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name} ({Classification}) [{Database}]";
        }
    }
}
