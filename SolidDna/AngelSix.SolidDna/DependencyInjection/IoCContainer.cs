using Dna;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using static Dna.FrameworkDI;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Access to available IoC services
    /// </summary>
    public static class IoC
    {
        #region Specific Dependency shortcuts

        /// <summary>
        /// Access the <see cref="ILocalizationManager"/>
        /// </summary>
        public static ILocalizationManager Localization => Get<ILocalizationManager>();

        /// <summary>
        /// The instance of the <see cref="AddInIntegration"/> class that is used for this add-in
        /// </summary>
        public static AddInIntegration AddIn => Get<AddInIntegration>();

        #endregion

        #region Method shortcuts

        /// <summary>
        /// Attempts to get the injected service of the specified type
        /// </summary>
        /// <typeparam name="T">The type of service to fetch</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return IoCContainer.Get<T>();
        }

        /// <summary>
        /// Attempts to get the injected service of the specified type
        /// </summary>
        /// <param name="type">The type of service to fetch</param>
        /// <returns></returns>
        public static object Get(Type type)
        {
            return IoCContainer.Get(type);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets up the IoC and injects all required elements
        /// </summary>
        /// <param name="addinPath">The full path to the add-in dll file</param>
        /// <param name="configureServices">Provides a callback to inject any services into the Dna.Framework DI system</param>
        public static void Setup(string addinPath, Action<FrameworkConstruction> configureServices = null)
        {
            // Create default construction
            Framework.Construct(new DefaultFrameworkConstruction(configure =>
            {
                // If the add-in path is not null
                if (!string.IsNullOrEmpty(addinPath))
                    // Add configuration file for the name of this file
                    // For example if it is MyAddin.dll then the configuration file
                    // will be in the same folder called MyAddin.appsettings.json"
                    configure.AddJsonFile(Path.ChangeExtension(addinPath, "appsettings.json"), optional: true);
            }));

            // Invoke the callback for adding custom services
            configureServices?.Invoke(Framework.Construction);

            // Build DI
            Framework.Construction.Build();
        }

        #endregion 
    }

    /// <summary>
    /// Access to available IoC services
    /// </summary>
    public static class IoCContainer
    {
        #region Public Methods

        /// <summary>
        /// Attempts to get the injected service of the specified type
        /// </summary>
        /// <typeparam name="T">The type of service to fetch</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            try
            {
                return Framework.Service<T>();
            }
            catch (Exception ex)
            {
                Logger?.LogCriticalSource($"Get '{typeof(T)}' failed. {ex.GetErrorMessage()}");
                return default;
            }
        }

        /// <summary>
        /// Attempts to get the injected service of the specified type
        /// </summary>
        /// <param name="type">The type of service to fetch</param>
        /// <returns></returns>
        public static object Get(Type type)
        {
            try
            {
                return Framework.Provider.GetService(type);
            }
            catch (Exception ex)
            {
                Logger?.LogCriticalSource($"Get '{type}' failed. {ex.GetErrorMessage()}");
                return null;
            }
        }

        #endregion
    }
}
