namespace AngelSix.SolidDna
{
    /// <summary>
    /// The type of resource used in a localization manager
    /// </summary>
    public enum ResourceDefinitionType
    {
        /// <summary>
        /// The resource is embedded in the assembly
        /// </summary>
        EmbeddedResource = 1,

        /// <summary>
        /// The resource is on the file system of the application
        /// </summary>
        File = 2,

        /// <summary>
        /// The resource is a remote URL
        /// </summary>
        Url = 3
    }

    /// <summary>
    /// The definition of a resource used in a localization manager
    /// </summary>
    public class ResourceDefinition
    {
        /// <summary>
        /// The type of resource
        /// </summary>
        public ResourceDefinitionType Type { get; set; }

        /// <summary>
        /// If set, this is the format of the file. If not specified, the file extension is used
        /// </summary>
        public string ExplicitFormat { get; set; }

        /// <summary>
        /// The location of the resource, either an URL, an embedded path or file system path
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// If true and no resource file is found for the specified culture,
        /// the default culture file is used instead
        /// </summary>
        public bool UseDefaultCultureIfNotFound { get; set; }
    }
}
