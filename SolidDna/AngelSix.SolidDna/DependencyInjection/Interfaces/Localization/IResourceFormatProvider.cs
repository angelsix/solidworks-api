using System;
using System.Threading.Tasks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A resource format provider for use with an ILocalizationManager
    /// </summary>
    public interface IResourceFormatProvider
    {
        /// <summary>
        /// Determines if the specified format is supported by this provider
        /// </summary>
        bool SupportsFormat(string format);

        /// <summary>
        /// Finds a string of the given name, taking into account the culture information.
        /// If no culture is specified, the default culture is used
        /// 
        /// IMPORTANT:
        /// NOTE: Make sure any and all await calls inside this function and its children
        ///       use ConfigureAwait(false). This is because the parent has to support 
        ///       a synchronous version of this call, so the method cannot sync back with
        ///       it's calling context without risk of deadlock
        /// </summary>
        /// <param name="resource">The resource definition</param>
        /// <param name="name">The name of the resource to find</param>
        /// <param name="culture">The culture information to use</param>
        /// <param name="onResult">Called with the value if successful</param>
        /// <returns>Returns true if fetching the value was successful</returns>
        Task<bool> GetStringAsync(ResourceDefinition resource, string name, string culture, Action<string> onResult);
    }
}
