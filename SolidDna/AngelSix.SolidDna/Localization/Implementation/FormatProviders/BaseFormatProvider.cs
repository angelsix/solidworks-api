using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A base class adding caching ability and helper functions to IResourceFormatProvider implementations
    /// </summary>
    public class BaseFormatProvider
    {
        #region Protected Members

        /// <summary>
        /// A lock to limit access to the processes to one at a time
        /// </summary>
        private readonly SemaphoreSlim mSelfLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// A list of all cached resources
        /// </summary>
        protected Dictionary<string, object> mCache;

        /// <summary>
        /// If true, resource files are cached in memory
        /// </summary>
        protected bool mCacheResourceFiles;

        #endregion

        #region Protected Helpers

        /// <summary>
        /// Gets the file data for the specified resource, storing a cached version if specified.
        /// IMPORTANT:
        /// NOTE: Make sure any and all await calls inside this function and its children
        ///       use ConfigureAwait(false). This is because the parent has to support 
        ///       a synchronous version of this call, so the method cannot sync back with
        ///       its calling context without risk of deadlock.
        /// </summary>
        /// <param name="pathFormat">The path to the desired resource, containing {0} in place of the culture</param>
        /// <param name="constructData">The function that takes the file data and converts it into the format required by the provider</param>
        /// <param name="culture">The culture to use. If not specified, the default culture is used</param>
        /// <returns></returns>
        protected async Task<T> GetResourceDocumentAsync<T>(ResourceDefinition pathFormat, Func<Stream, T> constructData, string culture = null)
        {
            try
            {
                await mSelfLock.WaitAsync();

                // Get culture path 
                var resourcePath = ResourceFormatProviderHelpers.GetCulturePath(pathFormat.Location, culture);

                // Make sure list has been initialized
                if (mCache == null)
                    mCache = new Dictionary<string, object>();

                // If we have a document already, return that
                if (mCache.ContainsKey(resourcePath))
                    return (T)mCache[resourcePath];

                // Otherwise try and get it
                var resourceDocument = default(T);

                try
                {
                    // Try to get the stream for this resource
                    using (var stream = await ResourceFormatProviderHelpers.GetStreamAsync(pathFormat.Type, resourcePath).ConfigureAwait(false))
                    {
                        // If successful, try and convert that data into a usable resource object
                        if (stream != null)
                            resourceDocument = constructData(stream);
                    }
                }
                finally
                {
                    // Add resource document if found or not (except for Urls, never add failed Urls in case of bad Internet connection
                    if (mCacheResourceFiles && pathFormat.Type != ResourceDefinitionType.Url)
                        mCache.Add(resourcePath, resourceDocument);
                }

                // Return what we found, if anything
                return resourceDocument;
            }
            finally
            {
                mSelfLock.Release();
            }
        }

        #endregion
    }
}
