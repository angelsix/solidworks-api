using Dna;
using Microsoft.Extensions.DependencyInjection;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Extension methods for adding DI implementations
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds the Localization manage to the DI
        /// </summary>
        /// <param name="construction">The construction</param>
        /// <returns></returns>
        public static FrameworkConstruction AddLocalizationManager(this FrameworkConstruction construction)
        {
            // Add the localization manager
            construction.Services.AddSingleton<ILocalizationManager>(new LocalizationManager
             {
                 StringResourceDefinition = new ResourceDefinition
                 {
                     Type = ResourceDefinitionType.EmbeddedResource,
                     Location = "AngelSix.SolidDna.Localization.Strings.Strings-{0}.xml",
                     UseDefaultCultureIfNotFound = true,
                 }
             });

            // Return for chaining
            return construction;
        }
    }
}
