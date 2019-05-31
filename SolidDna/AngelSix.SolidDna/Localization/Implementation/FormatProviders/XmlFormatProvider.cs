using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A resource provider that supports XML structured documents
    /// </summary>
    public class XmlFormatProvider : BaseFormatProvider, IResourceFormatProvider
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public XmlFormatProvider()
        {
            mCacheResourceFiles = true;
        }

        #endregion

        #region Public Methods

        public bool SupportsFormat(string format)
        {
            // Supports XML extensions
            if (string.IsNullOrEmpty(format))
                return false;

            return string.Equals("xml", format, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Finds a string of the given name, taking into account the culture information.
        /// If no culture is specified, the default culture is used
        /// 
        /// IMPORTANT:
        /// NOTE: Make sure any and all await calls inside this function and its children
        ///       use ConfigureAwait(false). This is because the parent has to support 
        ///       a synchronous version of this call, so the method cannot sync back with
        ///       its calling context without risk of deadlock.
        /// </summary>
        public async Task<bool> GetStringAsync(ResourceDefinition resource, string name, string culture, Action<string> onResult)
        {
            // Get string document for this culture
            var document = await GetResourceDocumentAsync(resource, XDocument.Load, culture).ConfigureAwait(false);

            // If it is null, try and find the default culture if specified
            if (document == null)
            {
                if (resource.UseDefaultCultureIfNotFound)

                    document = await GetResourceDocumentAsync(resource, XDocument.Load).ConfigureAwait(false);

                // Document not found so return false
                if (document == null)
                    return false;
            }

            // Null check for root document
            if (document.Root == null)
                return false;

            // Get the first element that matches the given name (ignore case)
            var element = document.Root.Elements().FirstOrDefault(
                f => f.Attributes().Any(a => a.Name.LocalName == "Name") && 
                string.Equals(name, f.Attributes().First(a => a.Name.LocalName == "Name").Value, StringComparison.CurrentCultureIgnoreCase));

            // Return false if not found
            if (element == null)
                return false;

            // Otherwise we found the value so return it
            onResult(element.Value);

            return true;
        }

        #endregion
    }
}
