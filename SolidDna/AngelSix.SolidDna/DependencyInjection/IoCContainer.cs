using Ninject;
using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Access to available IoC services
    /// </summary>
    public static class IoC
    {
        #region Specific Dependecy shortcuts

        /// <summary>
        /// Access the <see cref="ILocalizationManager"/>
        /// </summary>
        public static ILocalizationManager Localization { get { return IoCContainer.Get<ILocalizationManager>(); } }

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
        /// <typeparam name="type">The type of service to fetch</typeparam>
        /// <returns></returns>
        public static object Get(Type type)
        {
            return IoCContainer.Get(type);
        }

        #endregion
    }

    /// <summary>
    /// Access to available IoC services
    /// </summary>
    public static class IoCContainer
    {
        #region Private Members

        /// <summary>
        /// A flag indicating if the IoC container has been initialized
        /// </summary>
        private static bool mInitialized = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// The underlying kernel of the dependency injection handler
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        #endregion

        #region Public Methods

        /// <summary>
        /// Should be called to configure and inject all core services into the IoC Container at the very start of the application
        /// </summary>
        public static void Ensure()
        {
            if (mInitialized)
                return;

            // Add logger
            Kernel.Bind<ILogFactory>().ToConstant(new BaseLogFactory());

            // Add localization manager
            Kernel.Bind<ILocalizationManager>().ToConstant(new LocalizationManager
            {
                StringResourceDefinition = new ResourceDefinition
                {
                    Type = ResourceDefinitionType.EmbeddedResource,
                    Location = "AngelSix.SolidDna.Localization.Strings.Strings ({0}).xml",
                    UseDefaultCultureIfNotFound = true,
                }
            });

            mInitialized = true;
        }

        /// <summary>
        /// Attempts to get the injected service of the specified type
        /// </summary>
        /// <typeparam name="T">The type of service to fetch</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            try
            {
                // Check we have been initialized
                Ensure();

                return Kernel.Get<T>();
            }
            catch (Exception ex)
            {
                Logger.Log($"Get '{typeof(T)}' failed. {ex.GetErrorMessage()}");
                return default(T);
            }
        }

        /// <summary>
        /// Attempts to get the injected service of the specified type
        /// </summary>
        /// <typeparam name="type">The type of service to fetch</typeparam>
        /// <returns></returns>
        public static object Get(Type type)
        {
            try
            {
                // Check we have been initialized
                Ensure();

                return Kernel.Get(type);
            }
            catch (Exception ex)
            {
                Logger.Log($"Get '{type}' failed. {ex.GetErrorMessage()}");
                return null;
            }
        }

        #endregion
    }
}
