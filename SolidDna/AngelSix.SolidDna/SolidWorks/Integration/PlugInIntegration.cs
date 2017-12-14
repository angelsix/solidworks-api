using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Provides functions related to SolidDna plug-ins
    /// </summary>
    public static class PlugInIntegration
    {
        #region Private Members

        /// <summary>
        /// The AppDomain used to load and unload plug-ins
        /// </summary>
        private static AppDomain PlugInAppDomain;

        /// <summary>
        /// The cross-domain marshal to use for the plug-in Application domain calls
        /// </summary>
        private static PlugInIntegrationMarshal CrossDomain;

        #endregion

        #region Public Properties

        /// <summary>
        /// A list of available plug-ins loaded once SolidWorks has connected
        /// </summary>
        public static List<SolidPlugIn> PlugIns = new List<SolidPlugIn>();

        /// <summary>
        /// If true, will load your Add-in dll in it's own application domain so you can 
        /// unload and rebuild your add-in without having to close SolidWorks
        /// NOTE: This does seem to expose some bugs and issues in SolidWorks API
        ///       in terms of resolving references to specific dll's, so if you experience
        ///       issues try turning this off
        /// </summary>
        public static bool UseDetachedAppDomain { get; set; } = false;

        /// <summary>
        /// A list of assembly full names to resolve across domains, excluding anything else that may be found in <see cref="PlugInDetails"/>
        /// </summary>
        public static List<string> AssembliesToResolve { get; set; } = new List<string>();

        /// <summary>
        /// If true, attempts to resolve all assemblies
        /// </summary>
        public static bool ResolveAllAssemblies { get; set; }

        /// <summary>
        /// A list of all plug-ins that have been added to be loaded
        /// The key is the absolute file path, and the Type is the <see cref="SolidPlugIn"/> implementation type
        /// </summary>
        public static Dictionary<string, List<PlugInDetails>> PlugInDetails { get; private set; } = new Dictionary<string, List<PlugInDetails>>();

        /// <summary>
        /// The location of the SolidDna dll file and any plug-in's
        /// </summary>
        public static string PlugInFolder => typeof(PlugInIntegration).CodeBaseNormalized();

        /// <summary>
        /// The cross-domain marshal to use for the plug-in Application domain calls
        /// </summary>
        public static PlugInIntegrationMarshal PluginCrossDomain => CrossDomain;

        #endregion

        #region Public Events

        /// <summary>
        /// Called when a SolidWorks callback is fired
        /// </summary>
        public static event Action<string> CallbackFired = (name) => { };

        #endregion

        #region Setup / Tear down

        /// <summary>
        /// Must be called to setup the PlugInIntegration
        /// </summary>
        public static void Setup(string revisionNumber, int cookie)
        {
            if (UseDetachedAppDomain)
            {
                // Make sure we resolve assemblies in this domain, as it seems to use this domain to resolve
                // assemblies not the appDomain when crossing boundaries
                AppDomain.CurrentDomain.AssemblyResolve += PlugInIntegrationMarshal.AppDomain_AssemblyResolve;

                PlugInAppDomain = AppDomain.CreateDomain("SolidDnaPlugInDomain", null, new AppDomainSetup
                {
                    // Use plug-in folder for resolving plug-ins
                    ApplicationBase = PlugInFolder,
                });

                // Make sure we load our own marshal
                AssembliesToResolve.Add(typeof(PlugInIntegrationMarshal).Assembly.FullName);

                // Run code on new app-domain to configure
                CrossDomain = (PlugInIntegrationMarshal)PlugInAppDomain.CreateInstanceAndUnwrap(typeof(PlugInIntegrationMarshal).Assembly.FullName, typeof(PlugInIntegrationMarshal).FullName);

                // Setup
                CrossDomain.SetupAppDomain(revisionNumber, cookie);
            }
            else
            {
                // Setup IoC
                IoCContainer.Ensure();

                // Get the version number (such as 25 for 2016)
                var postFix = "";
                if (revisionNumber != null && revisionNumber.Contains("."))
                    postFix = "." + revisionNumber.Substring(0, revisionNumber.IndexOf('.'));

                // Store a reference to the current SolidWorks instance
                // Initialize SolidWorks (SolidDNA class)
                AddInIntegration.SolidWorks = new SolidWorksApplication((SldWorks)Activator.CreateInstance(Type.GetTypeFromProgID("SldWorks.Application" + postFix)), cookie);
            }
        }

        /// <summary>
        /// Cleans up the plug-in app domain so that the plug-in dll files can be edited after unloading
        /// </summary>
        public static void Teardown()
        {
            // Run code on new app-domain to tear down
            if (UseDetachedAppDomain)
            {
                // Tear down
                CrossDomain.Teardown();

                // Unload our domain
                AppDomain.Unload(PlugInAppDomain);
            }
            else
            {
                // Dispose SolidWorks COM
                AddInIntegration.SolidWorks?.Dispose();
                AddInIntegration.SolidWorks = null;
            }
        }

        #endregion

        #region Connected to SolidWorks

        /// <summary>
        /// Called when the add-in has connected to SolidWorks
        /// </summary>
        public static void ConnectedToSolidWorks()
        {
            if (UseDetachedAppDomain)
                CrossDomain.ConnectedToSolidWorks();
            else
            {
                AddInIntegration.OnConnectedToSolidWorks();

                // Inform plug-ins
                PlugIns.ForEach(plugin => plugin.ConnectedToSolidWorks());
            }
        }

        /// <summary>
        /// Called when the add-in has disconnected from SolidWorks
        /// </summary>
        public static void DisconnectedFromSolidWorks()
        {
            if (UseDetachedAppDomain)
                CrossDomain.DisconnectedFromSolidWorks();
            else
            {
                AddInIntegration.OnDisconnectedFromSolidWorks();

                // Inform plug-ins
                PlugIns.ForEach(plugin => plugin.DisconnectedFromSolidWorks());
            }
        }

        #endregion

        #region Add Plug-in

        /// <summary>
        /// Adds a plug-in based on it's <see cref="SolidPlugIn"/> implementation
        /// </summary>
        /// <typeparam name="T">The class that implements the <see cref="SolidPlugIn"/></typeparam>
        /// </param>
        public static void AddPlugIn<T>()
        {
            if (UseDetachedAppDomain)
                CrossDomain.AddPlugIn<T>();
            else
            {
                // Get the full path to the assembly
                var fullPath = typeof(T).Assembly.CodeBase.Replace(@"file:\", "").Replace(@"file:///", "");

                // Create list if one doesn't exist
                if (!PlugInDetails.ContainsKey(fullPath))
                    PlugInDetails[fullPath] = new List<PlugInDetails>();

                // Add it
                PlugInDetails[fullPath].Add(new PlugInDetails
                {
                    FullPath = fullPath,
                    AssemblyFullName = AssemblyName.GetAssemblyName(fullPath).FullName,
                    TypeFullName = typeof(T).FullName,
                });
            }
        }

        /// <summary>
        /// Adds a plug-in based on it's <see cref="SolidPlugIn"/> implementation
        /// </summary>
        /// <typeparam name="fullPath">The absolute path to the plug-in dll</typeparam>
        public static void AddPlugIn(string fullPath)
        {
            if (UseDetachedAppDomain)
                // Add it to the plug-in integration domain also
                CrossDomain.AddPlugIn(fullPath);
            else
            {
                // Create list if one doesn't exist
                if (!PlugInDetails.ContainsKey(fullPath))
                    PlugInDetails[fullPath] = new List<PlugInDetails>();

                List<PlugInDetails> plugins;

                plugins = GetPlugInDetails(fullPath);

                // Add any found plug-ins
                if (plugins?.Count > 0)
                    PlugInDetails[fullPath].AddRange(plugins);
            }
        }

        #endregion

        #region SolidWorks Callbacks

        public static void OnCallback(string name)
        {
            // Inform plug-in domain of event
            if (UseDetachedAppDomain)
                CrossDomain.OnCallback(name);
            else
            {
                // Inform listeners
                CallbackFired(name);
            }
        }

        #endregion

        #region Configure Plug Ins

        /// <summary>
        /// Discovers all SolidDna plug-ins
        /// </summary>
        /// <param name="loadAll">True to find all plug-ins in the same folder as the SolidDna dll</param>
        /// <returns></returns>
        public static List<SolidPlugIn> SolidDnaPlugIns(bool loadAll = true)
        {
            // Create new empty list
            var assemblies = new List<SolidPlugIn>();

            // Find all dll's in the same directory
            if (loadAll)
            { 
                if (UseDetachedAppDomain)
                {
                    // Invalid combination... cannot load all from cross domain
                    // (we don't create the PlugInDetails class for each item
                    Debugger.Break();
                }

                // Clear old list
                PlugInDetails = new Dictionary<string, List<PlugInDetails>>();

                // Add new based on if found
                foreach (var path in Directory.GetFiles(PlugInFolder, "*.dll", SearchOption.TopDirectoryOnly))
                    GetPlugIns(path, (plugin) => assemblies.Add(plugin));
            }
            // Or load explicit ones
            else
            {
                // For each assembly
                foreach (var p in PlugInDetails)
                {
                    // And each plug-in inside it
                    foreach (var path in p.Value)
                    {
                        try
                        {
                            // If we are called in the main domain, cross-load
                            if (UseDetachedAppDomain)
                            {
                                // Create instance of the plug-in via cross-domain and cast back
                                var plugin = (dynamic)PlugInAppDomain.CreateInstanceAndUnwrap(
                                                        path.AssemblyFullName,
                                                        path.TypeFullName);

                                // If we got it, add it to the list
                                if (plugin != null)
                                    assemblies.Add(plugin);
                            }
                            else
                            {
                                GetPlugIns(path.FullPath, (plugin) => assemblies.Add(plugin));
                            }
                        }
                        catch
                        {
                            // TOOD: Report error
                        }
                    }
                }
            }

            return assemblies;
        }

        /// <summary>
        /// Loads the dll into the current app domain, and finds any <see cref="SolidPlugIn"/> implementations, calling onFound when it finds them
        /// </summary>
        /// <param name="pluginFullPath">The full path to the plug-in dll to load</param>
        /// <param name="onFound">Called when a <see cref="SolidPlugIn"/> is found</param>
        public static void GetPlugIns(string pluginFullPath, Action<SolidPlugIn> onFound)
        {
            // Load the assembly
            var assembly = Assembly.LoadFile(pluginFullPath);

            // If we didn't succeed, ignore
            if (assembly == null)
                return;

            var type = typeof(SolidPlugIn);

            // Find all types in an assembly. Catch assemblies that don't allow this.
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException)
            {
                return;
            }

            // See if any of the type are of SolidPlugIn
            types.Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList().ForEach(p =>
            {
                // Create SolidDna plugin class instance
                if (Activator.CreateInstance(p) is SolidPlugIn inter)
                    onFound(inter);
            });
        }

        /// <summary>
        /// Loads the assembly, finds all <see cref="SolidPlugIn"/> implementations and 
        /// creates a list of <see cref="PlugInDetails"/> for them
        /// </summary>
        /// <param name="fullPath">The assembly full path to load</param>
        /// <returns></returns>
        public static List<PlugInDetails> GetPlugInDetails(string fullPath)
        {
            var list = new List<PlugInDetails>();

            GetPlugIns(fullPath, (plugin) => list.Add(new PlugInDetails
            {
                AssemblyFullName = plugin.GetType().AssemblyBaseNormalized(),
                FullPath = fullPath,
                TypeFullName = plugin.GetType().FullName
            }));

            return list;
        }

        /// <summary>
        /// Runs any initialization code required on plug-ins
        /// </summary>
        public static void ConfigurePlugIns()
        {
            if (UseDetachedAppDomain)
                CrossDomain.ConfigurePlugIns();
            else
            {
                // This is usually run for the ComRegister function

                // Try and find the title from the first plug-in found
                var plugins = SolidDnaPlugIns(loadAll: true);
                if (plugins.Count > 0)
                {
                    AddInIntegration.SolidWorksAddInTitle = plugins.First().AddInTitle;
                    AddInIntegration.SolidWorksAddInDescription = plugins.First().AddInDescription;
                }

                // Load all plug-in's at this stage for faster lookup
                PlugIns = SolidDnaPlugIns();
            }
        }

        #endregion
    }
}
