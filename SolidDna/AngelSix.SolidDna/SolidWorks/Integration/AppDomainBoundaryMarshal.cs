using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// AppDomain cross-boundary caller methods
    /// </summary>
    public class AppDomainBoundaryMarshal : MarshalByRefObject
    {
        /// <summary>
        /// Configures the application IoC
        /// </summary>
        /// <param name="assemblyFilePath">The path to the assembly</param>
        /// <param name="pathToConfigureDll">Absolute path to dll where the IConfigureServices implementation lies</param>
        /// <param name="configureName">The name of the <see cref="ConfigureServiceAttribute"/> method to use</param>
        public void SetupIoC(string assemblyFilePath, string pathToConfigureDll = null, string configureName = null)
        {
            AppDomainBoundary.SetupIoC(assemblyFilePath, pathToConfigureDll, configureName);
        }

        #region Plugin Integration

        /// <summary>
        /// Must be called to setup the PlugInIntegration
        /// </summary>
        /// <param name="addinPath">The path to the add-in that is calling this setup (typically acquired using GetType().Assembly.Location)</param>
        /// <param name="cookie">The cookie Id of the SolidWorks instance</param>
        /// <param name="version">The version of the currently connected SolidWorks instance</param>
        public void PluginIntegrationSetup(string addinPath, string version, int cookie)
        {
            PlugInIntegration.Setup(addinPath, version, cookie);
        }

        /// <summary>
        /// Tears down the app-domain that the plug-ins run inside of
        /// </summary>
        public void PluginIntegrationTeardown()
        {
            PlugInIntegration.Teardown();
        }

        #endregion

        #region Life-cycle Management

        /// <summary>
        /// Make this cross domain object last forever so GC doesn't dispose of it during inactivity
        /// </summary>
        /// <returns></returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        #endregion

        #region Connected to SolidWorks 

        /// <summary>
        /// Called when the add-in has connected to SolidWorks
        /// </summary>
        public void ConnectedToSolidWorks()
        {
            PlugInIntegration.ConnectedToSolidWorks();
        }

        /// <summary>
        /// Called when the add-in has disconnected from SolidWorks
        /// </summary>
        public void DisconnectedFromSolidWorks()
        {
            PlugInIntegration.DisconnectedFromSolidWorks();
        }

        #endregion

        #region SolidWorks Callbacks

        /// <summary>
        /// Called by the SolidWorks domain (AddInIntegration) when a callback is fired
        /// </summary>
        /// <param name="name">The parameter passed into the generic callback</param>
        public void OnCallback(string name)
        {
            // Let listeners know
            PlugInIntegration.OnCallback(name);
        }

        #endregion

        #region Plug-Ins

        /// <summary>
        /// Runs any initialization code required on plug-ins
        /// </summary>
        /// <param name="addinPath">The path to the add-in that is calling this setup (typically acquired using GetType().Assembly.Location)</param>
        public void ConfigurePlugIns(string addinPath)
        {
            PlugInIntegration.ConfigurePlugIns(addinPath);
        }

        /// <summary>
        /// Adds a plug-in based on its <see cref="SolidPlugIn"/> implementation
        /// </summary>
        /// <param name="fullPath">The absolute path to the plug-in dll</param>
        public void AddPlugIn(string fullPath)
        {
            PlugInIntegration.AddPlugIn(fullPath);
        }

        /// <summary>
        /// Adds a plug-in based on its <see cref="SolidPlugIn"/> implementation
        /// </summary>
        /// <typeparam name="T">The class that implements the <see cref="SolidPlugIn"/></typeparam>
        public void AddPlugIn<T>()
        {
            PlugInIntegration.AddPlugIn<T>();
        }

        /// <summary>
        /// Loads the assembly, finds all <see cref="SolidPlugIn"/> implementations and 
        /// creates a list of <see cref="PlugInDetails"/> for them
        /// </summary>
        /// <param name="fullPath">The assembly full path to load</param>
        /// <returns></returns>
        public List<PlugInDetails> GetPlugInDetails(string fullPath)
        {
            return PlugInIntegration.GetPlugInDetails(fullPath);
        }

        #endregion
    }
}
