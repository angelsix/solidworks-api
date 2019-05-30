using System.Collections.Generic;
using System.Threading.Tasks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A resource manager that supports localization
    /// </summary>
    public interface ILocalizationManager
    {
        /// <summary>
        /// The default culture to use. If not specified the system default is used
        /// </summary>
        string DefaultCulture { get; set; }

        /// <summary>
        /// The path to the string resource files, containing {0} where the culture name is
        /// </summary>
        ResourceDefinition StringResourceDefinition { get; set; }

        /// <summary>
        /// A list of providers this manager can call upon to decode resources
        /// </summary>
        List<IResourceFormatProvider> Providers { get; set; }

        /// <summary>
        /// Finds a string of the given name, taking into account the culture information.
        /// If no culture is specified, the default culture is used
        /// 
        /// IMPORTANT:
        /// NOTE: Make sure any and all await calls inside this function and its children
        ///       use ConfigureAwait(false). This is because the parent has to support 
        ///       a synchronous version of this call, so the method cannot sync back with
        ///       its calling context without risk of deadlock
        /// </summary>
        /// <param name="name">The name of the resource to find</param>
        /// <param name="culture">The culture information to use</param>
        /// <returns>Returns the string if found, or null if not found</returns>
        Task<string> GetStringAsync(string name, string culture = null);
    }
}
