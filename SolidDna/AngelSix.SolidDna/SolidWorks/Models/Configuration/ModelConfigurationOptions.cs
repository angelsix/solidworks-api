namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a configuration option used in multiple calls
    /// 
    /// NOTE: Known types are here http://help.solidworks.com/2019/english/api/swconst/SOLIDWORKS.Interop.swconst~SOLIDWORKS.Interop.swconst.swInConfigurationOpts_e.html
    /// 
    /// </summary>
    public enum ModelConfigurationOptions
    {
        /// <summary>
        /// Suppress Features in Configuration Property
        /// </summary>
        ConfigPropertySuppressFeatures = 0,

        /// <summary>
        /// This configuration
        /// </summary>
        ThisConfiguration = 1,

        /// <summary>
        /// All configurations
        /// </summary>
        AllConfiguration = 2,

        /// <summary>
        /// Specific configuration
        /// </summary>
        SpecificConfiguration = 3,

        /// <summary>
        /// Linked to parent
        /// </summary>
        /// <remarks>
        ///     Valid only for derived configurations;
        ///     if specified for non-derived configurations, 
        ///     then the active configuration is used
        /// </remarks>
        LinkedToParent = 4,

        /// <summary>
        /// Speedpak Configuration
        /// </summary>
        SpeedpakConfiguration = 5
    }
}
