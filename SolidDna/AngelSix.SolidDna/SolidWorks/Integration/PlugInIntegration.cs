﻿using Dna;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using static Dna.FrameworkDI;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Provides functions related to SolidDna plug-ins
    /// </summary>
    public static class PlugInIntegration
    {
        #region Public Properties

        /// <summary>
        /// A list of all plug-ins that have been added to be loaded. 
        /// The key is the absolute file path, and the Type is the <see cref="SolidPlugIn"/> implementation type
        /// </summary>
        public static Dictionary<string, List<PlugInDetails>> PlugInDetails { get; private set; } = new Dictionary<string, List<PlugInDetails>>();

        /// <summary>
        /// If true, searches in the directory of the application (where AngelSix.SolidDna.dll is) for any dll that
        /// contains any <see cref="SolidPlugIn"/> implementations and adds them to the <see cref="PlugInDetails"/>
        /// during the <see cref="ConfigurePlugIns(string, SolidAddIn)"/> stage.
        /// If false, the user should during the <see cref="SolidAddIn.PreLoadPlugIns"/> method, add
        /// any specific implementations of the <see cref="SolidPlugIn"/> to <see cref="PlugInIntegration.PlugInDetails"/> list
        /// </summary>
        public static bool AutoDiscoverPlugins { get; set; } = true;

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
        /// <param name="addinPath">The path to the add-in that is calling this setup (typically acquired using GetType().Assembly.Location)</param>
        /// <param name="cookie">The cookie Id of the SolidWorks instance</param>
        /// <param name="version">The version of the currently connected SolidWorks instance</param>
        public static void Setup(string addinPath, string version, int cookie)
        {
            // If use detached app domain...
            if (AppDomainBoundary.UseDetachedAppDomain)
                // Get boundary to re-call us from other app domain
                AppDomainBoundary.PluginIntegrationSetup(addinPath, version, cookie);
            else
            {
                // Log it
                Logger?.LogDebugSource($"PlugIn Setup...");

                // Get the version number (such as 25 for 2016)
                var postFix = "";
                if (version != null && version.Contains("."))
                    postFix = "." + version.Substring(0, version.IndexOf('.'));

                // Store a reference to the current SolidWorks instance
                // Initialize SolidWorks (SolidDNA class)
                AddInIntegration.SolidWorks = new SolidWorksApplication((SldWorks)Activator.CreateInstance(Type.GetTypeFromProgID("SldWorks.Application" + postFix)), cookie);

                // Log it
                Logger?.LogDebugSource($"SolidWorks Instance Created? {AddInIntegration.SolidWorks != null}");
            }
        }

        /// <summary>
        /// Cleans up the plug-in app domain so that the plug-in dll files can be edited after unloading
        /// </summary>
        public static void Teardown()
        {
            // If use detached app domain...
            if (AppDomainBoundary.UseDetachedAppDomain)
                // Get boundary to re-call us from other app domain
                AppDomainBoundary.PluginIntegrationTeardown();
            else
            {
                // Nothing to do right now
            }
        }

        #endregion

        #region Connected to SolidWorks

       /// <summary>
       /// Called when the add-in has connected to SolidWorks
       /// </summary>
       /// <param name="solidAddIn"></param>
        public static void ConnectedToSolidWorks(SolidAddIn solidAddIn)
        {
            // If use detached app domain...
            if (AppDomainBoundary.UseDetachedAppDomain)
                // Get boundary to re-call us from other app domain
                AppDomainBoundary.ConnectedToSolidWorks(solidAddIn);
            else
            {
                solidAddIn.OnConnectedToSolidWorks();

                // Inform plug-ins
                solidAddIn.PlugIns.ForEach(plugin =>
                {
                    // Log it
                    Logger?.LogDebugSource($"Firing ConnectedToSolidWorks event for plugin `{plugin.AddInTitle}`...");

                    plugin.ConnectedToSolidWorks();
                });
            }
        }

        /// <summary>
        /// Called when the add-in has disconnected from SolidWorks
        /// </summary>
        /// <param name="solidAddIn"></param>
        public static void DisconnectedFromSolidWorks(SolidAddIn solidAddIn)
        {
            // If use detached app domain...
            if (AppDomainBoundary.UseDetachedAppDomain)
                // Get boundary to re-call us from other app domain
                AppDomainBoundary.DisconnectedFromSolidWorks(solidAddIn);
            else
            {
                solidAddIn.OnDisconnectedFromSolidWorks();

                // Inform plug-ins
                solidAddIn.PlugIns.ForEach(plugin =>
                {
                    // Log it
                    Logger?.LogDebugSource($"Firing DisconnectedFromSolidWorks event for plugin `{plugin.AddInTitle}`...");

                    plugin.DisconnectedFromSolidWorks();
                });
            }
        }

        #endregion

        #region Add Plug-in

        /// <summary>
        /// Adds a plug-in based on its <see cref="SolidPlugIn"/> implementation
        /// </summary>
        /// <typeparam name="T">The class that implements the <see cref="SolidPlugIn"/></typeparam>
        public static void AddPlugIn<T>()
        {
            // If use detached app domain...
            if (AppDomainBoundary.UseDetachedAppDomain)
                // Get boundary to re-call us from other app domain
                AppDomainBoundary.AddPlugIn<T>();
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
        /// Adds a plug-in based on its <see cref="SolidPlugIn"/> implementation
        /// </summary>
        /// <param name="fullPath">The absolute path to the plug-in dll</param>
        public static void AddPlugIn(string fullPath)
        {
            // If use detached app domain...
            if (AppDomainBoundary.UseDetachedAppDomain)
                // Get boundary to re-call us from other app domain
                AppDomainBoundary.AddPlugIn(fullPath);
            else
            {
                // Don't auto discover plug-ins if we added manually
                AutoDiscoverPlugins = false;

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

        /// <summary>
        /// Called by the SolidWorks domain (AddInIntegration) when a callback is fired
        /// </summary>
        /// <param name="name">The parameter passed into the generic callback</param>
        public static void OnCallback(string name)
        {
            // If use detached app domain...
            if (AppDomainBoundary.UseDetachedAppDomain)
                // Get boundary to re-call us from other app domain
                AppDomainBoundary.OnCallback(name);
            else
            {
                try
                {
                    // Inform listeners
                    CallbackFired(name);
                }
                catch (Exception ex)
                {
                    Debugger.Break();

                    // Log it
                    Logger?.LogCriticalSource($"OnCallback failed. {ex.GetErrorMessage()}");
                }
            }
        }

        #endregion

        #region Configure Plug Ins

        /// <summary>
        /// Discovers all SolidDna plug-ins
        /// </summary>
        /// <param name="addinPath">The path to the add-in that is calling this setup (typically acquired using GetType().Assembly.Location)</param>
        /// <returns></returns>
        public static List<SolidPlugIn> SolidDnaPlugIns(string addinPath)
        {
            // Create new empty list
            var assemblies = new List<SolidPlugIn>();

            // Find all dll's in the same directory
            if (AutoDiscoverPlugins)
            {
                // Log it
                Logger?.LogDebugSource($"Loading all PlugIns...");

                if (AppDomainBoundary.UseDetachedAppDomain)
                {
                    // Invalid combination... cannot load all from cross domain
                    // (we don't create the PlugInDetails class for each item
                    Debugger.Break();
                }

                // Clear old list
                PlugInDetails = new Dictionary<string, List<PlugInDetails>>();

                // Add new based on if found
                foreach (var path in Directory.GetFiles(addinPath, "*.dll", SearchOption.TopDirectoryOnly))
                    GetPlugIns(path, (plugin) =>
                    {
                        // Log it
                        Logger?.LogDebugSource($"Found plugin {plugin.AddInTitle} in {path}");

                        assemblies.Add(plugin);
                    });
            }
            // Or load explicit ones
            else
            {
                // Log it
                Logger?.LogDebugSource($"Explicitly loading {PlugInDetails?.Count} PlugIns...");

                // For each assembly
                foreach (var p in PlugInDetails)
                {
                    // And each plug-in inside it
                    foreach (var path in p.Value)
                    {
                        try
                        {
                            // If we are called in the main domain, cross-load
                            if (AppDomainBoundary.UseDetachedAppDomain)
                            {
                                // Log it
                                Logger?.LogDebugSource($"Cross-domain loading PlugIn {path.AssemblyFullName}...");

                                // Create instance of the plug-in via cross-domain and cast back
                                var plugin = (dynamic)AppDomainBoundary.AppDomain.CreateInstanceAndUnwrap(
                                                        path.AssemblyFullName,
                                                        path.TypeFullName);

                                // If we got it...
                                if (plugin != null)
                                    // Add it to the list
                                    assemblies.Add(plugin);
                                // Otherwise...
                                else
                                    // Log error
                                    Logger?.LogErrorSource($"Failed to create instance of PlugIn {path.AssemblyFullName}");
                            }
                            else
                            {
                                // Try and find the SolidPlugIn implementation...
                                GetPlugIns(path.FullPath, (plugin) =>
                                {
                                    // Log it
                                    Logger?.LogDebugSource($"Found plugin {plugin.AddInTitle} in {path}");

                                    // Add it to the list
                                    assemblies.Add(plugin);
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log error
                            Logger?.LogCriticalSource($"Unexpected error: {ex}");
                        }
                    }
                }
            }

            // Log it
            Logger?.LogDebugSource($"Loaded {assemblies?.Count} plug-ins from {addinPath}");

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
            // NOTE: Calling LoadFrom instead of LoadFile will auto-resolve references in that folder
            //       otherwise they won't resolve.
            //       For this reason its important that plug-ins are in the same folder as the 
            //       AngelSix.SolidDna.dll and all other used references
            var assembly = Assembly.LoadFrom(pluginFullPath);

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
        /// <param name="addinPath">The path to the add-in that is calling this setup (typically acquired using GetType().Assembly.Location)</param>
        /// <param name="solidAddIn"></param>
        public static void ConfigurePlugIns(string addinPath, SolidAddIn solidAddIn)
        {
            // If use detached app domain...
            if (AppDomainBoundary.UseDetachedAppDomain)
            {
                // Get boundary to re-call us from other app domain
                AppDomainBoundary.ConfigurePlugIns(addinPath, solidAddIn);
            }
            else
            {
                // This is usually run for the ComRegister function

                // *********************************************************************************
                //
                // WARNING: 
                // 
                //   If SolidWorks is loading our add-ins and we have multiple that use SolidDna
                //   it loads and makes use of the existing AngelSix.SolidDna.dll file from
                //   the first add-in loaded and shares it for all future add-ins
                //
                //   This results in any static instances being shared and only one version 
                //   of SolidDna being usable on an individual SolidWorks instance 
                //
                //   I am not sure of the reason for this but I feel it is a bug in SolidWorks
                //   as changing the GUID of the AngelSix.SolidDna.dll assembly and its 
                //   Assembly and File versions doesn't change what gets loaded by SolidWorks
                //
                //   Perhaps when we make this a NuGet package the way it references may
                //   make it work. Until then the only thing to keep in mind is any
                //   static values inside the AngelSix.SolidDna class could be shared between
                //   add-ins so things like PlugIns list will come in here initially at this 
                //   point with the last PlugIns list from the previous add-in. This is not an
                //   issue here as we override it straight away before making use of it,
                //   but it is something to bare in mind until we find a better solution
                //          
                //
                // *********************************************************************************

                // Load all plug-in's at this stage for faster lookup
                solidAddIn.PlugIns = SolidDnaPlugIns(addinPath);

                // Log it
                Logger?.LogDebugSource($"{solidAddIn.PlugIns.Count} plug-ins found");

                // Find first plug-in in the list and use that as the title and description (for COM register)
                var firstPlugInWithTitle = solidAddIn.PlugIns.FirstOrDefault(f => !string.IsNullOrEmpty(f.AddInTitle));

                // If we have a title...
                if (firstPlugInWithTitle != null)
                {
                    // Log it
                    Logger?.LogDebugSource($"Setting Add-In Title:       {firstPlugInWithTitle.AddInTitle}");
                    Logger?.LogDebugSource($"Setting Add-In Description: {firstPlugInWithTitle.AddInDescription}");

                    // Set title and description details
                    solidAddIn.SolidWorksAddInTitle = firstPlugInWithTitle.AddInTitle;
                    solidAddIn.SolidWorksAddInDescription = firstPlugInWithTitle.AddInDescription;
                }
                // Otherwise
                else
                    // Log it
                    Logger?.LogDebugSource($"No PlugIn's found with a title.");
            }
        }

        #endregion
    }
}
