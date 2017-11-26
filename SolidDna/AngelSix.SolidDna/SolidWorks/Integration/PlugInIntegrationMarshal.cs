using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// AppDomain cross-boundary helpers
    /// </summary>
    public class PlugInIntegrationMarshal : MarshalByRefObject
    {
        #region AppDomain setup

        /// <summary>
        /// Configures the app-domain that the plug-ins run inside of
        /// </summary>
        /// <param name="version">The version of the currently connected SolidWorks instance</param>
        /// <param name="cookie">The cookie Id of the SolidWorks instance</param>
        public void SetupAppDomain(string version, int cookie)
        {
            PlugInIntegration.Setup(version, cookie);

            // Make sure we resolve assemblies in this domain, as it seems to use this domain to resolve
            // assemblies not the appDomain when crossing boundaries
            AppDomain.CurrentDomain.AssemblyResolve += AppDomain_AssemblyResolve;
        }

        /// <summary>
        /// Tears down the app-domain that the plug-ins run inside of
        /// </summary>
        public void Teardown()
        {
            PlugInIntegration.Teardown();
        }

        /// <summary>
        /// Helper to resolve assemblies over AppDomain boundaries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Assembly AppDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                // Only try and process SolidDna plug-ins
                var found = PlugInIntegration.ResolveAllAssemblies;

                if (!found)
                {
                    foreach (var val in PlugInIntegration.PlugInDetails.Keys)
                        if (val == args.Name)
                        {
                            found = true;
                            break;
                        }
                }

                // If we have it in assemblies to resolve, thats ok too
                if (!found)
                    found = PlugInIntegration.AssembliesToResolve.Any(f => f == args.Name);

                if (!found)
                    return null;

                var assembly = Assembly.Load(args.Name);
                if (assembly != null)
                    return assembly;

                throw new FileNotFoundException();
            }
            catch
            {
                //
                // Try to load by filename - split out the filename of the full assembly name
                // and append the base path of the original assembly (ie. look in the same directory)
                //
                // NOTE: this doesn't account for special search paths but then that never
                //       worked before either
                //
                var Parts = args.Name.Split(',');
                var File = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Parts[0].Trim() + ".dll";

                return Assembly.LoadFrom(File);
            }
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
            try
            {
                // Let listeners know
                PlugInIntegration.OnCallback(name);
            }
            catch (Exception ex)
            {
                Debugger.Break();

                // Log it
                Logger.Log($"OnCallback failed. {ex.GetErrorMessage()}");
            }
        }

        #endregion

        #region Plug-Ins

        public void ConfigurePlugIns()
        {
            PlugInIntegration.ConfigurePlugIns();
        }

        /// <summary>
        /// Adds a plug-in based on it's <see cref="SolidPlugIn"/> implementation
        /// </summary>
        /// <typeparam name="fullPath">The absolute path to the plug-in dll</typeparam>
        public void AddPlugIn(string fullPath)
        {
            PlugInIntegration.AddPlugIn(fullPath);
        }

        /// <summary>
        /// Adds a plug-in based on it's <see cref="SolidPlugIn"/> implementation
        /// </summary>
        /// <typeparam name="T">The class that implements the <see cref="SolidPlugIn"/></typeparam>
        /// </param>
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