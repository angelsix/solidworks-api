using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Generic helper functions for any IResourceFormatProviders to use
    /// </summary>
    public static class ResourceFormatProviderHelpers
    {
        /// <summary>
        /// Attempts to get a stream from the specified resource type and path
        /// 
        /// IMPORTANT:
        /// NOTE: Make sure any and all await calls inside this function and its children
        ///       use ConfigureAwait(false). This is because the parent has to support 
        ///       a synchronous version of this call, so the method cannot sync back with
        ///       it's calling context without risk of deadlock
        /// </summary>
        /// <param name="type">The type of resource</param>
        /// <param name="resourcePath">The location of the resource</param>
        /// <returns></returns>
        public static async Task<Stream> GetStreamAsync(ResourceDefinitionType type, string resourcePath)
        {
            switch (type)
            {
                case ResourceDefinitionType.EmbeddedResource:
                    //Use the below to see the strings to use to access resource files.

                    // NOTE: To find all resources
                    //string[] arr = AssemblyHelpers.CoreAssembly().GetManifestResourceNames();

                    // Get resource from embedded file
                    // NOTE: We store the strings localization in the same assembly as the Localization class so find it based on that
                    var assembly = typeof(Localization).Assembly;
                    return assembly.GetManifestResourceStream(resourcePath);

                case ResourceDefinitionType.File:

                    // Use the file manager to load the resource files
                    return File.OpenRead(resourcePath);

                case ResourceDefinitionType.Url:

                    // Get document from the web
                    var webClient = new HttpClient();
                    return await webClient.GetStreamAsync(resourcePath).ConfigureAwait(false);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the culture from the specified culture, or resorts to the default culture if not specified 
        /// </summary>
        /// <param name="culture">The international standard for the desired culture, such as en-GB, en-US, fr-FR etc...</param>
        /// <returns></returns>
        public static string GetCultureName(string culture = null)
        {
            return culture ?? IoC.Localization.DefaultCulture;
        }

        /// <summary>
        /// Takes a formatted path and a culture name and returns the path
        /// </summary>
        /// <param name="locationFormat">The path format to the resource</param>
        /// <param name="culture">The culture to use. If not specified, the default culture will be used</param>
        /// <returns></returns>
        public static string GetCulturePath(string locationFormat, string culture = null)
        {
            var cultureName = GetCultureName(culture);

            // Replace {0} with culture name
            return string.Format(locationFormat, cultureName);
        }

        /// <summary>
        /// Gets the format of this resource
        /// </summary>
        /// <param name="resourceDefinition">The resource definition</param>
        /// <returns></returns>
        public static string GetFormat(ResourceDefinition resourceDefinition)
        {
            var extension = resourceDefinition.ExplicitFormat;

            // If not, get it from the file extension
            if (string.IsNullOrEmpty(resourceDefinition.ExplicitFormat))
            {
                extension = Path.GetExtension(resourceDefinition.Location);
                if (extension != null && extension.StartsWith("."))
                    extension = extension.Substring(1);
            }

            return extension;
        }
    }
}
