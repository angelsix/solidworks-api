using Dna;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using static Dna.FrameworkDI;
using System.IO;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Integrates into SolidWorks as an add-in and registers for callbacks provided by SolidWorks
    /// 
    /// IMPORTANT: The class that overrides <see cref="ISwAddin"/> MUST be the same class that 
    /// contains the ComRegister and ComUnregister functions due to how SolidWorks loads add-ins
    /// </summary>
    public abstract class AddInIntegration : ISwAddin
    {
        #region Public Properties

        /// <summary>
        /// The title displayed for this SolidWorks Add-in
        /// </summary>
        public static string SolidWorksAddInTitle { get; set; } = "AngelSix SolidDna AddIn";

        /// <summary>
        /// The description displayed for this SolidWorks Add-in
        /// </summary>
        public static string SolidWorksAddInDescription { get; set; } = "All your pixels are belong to us!";

        /// <summary>
        /// Represents the current SolidWorks application
        /// </summary>
        public static SolidWorksApplication SolidWorks { get; set; }

        #endregion

        #region Public Events

        /// <summary>
        /// Called once SolidWorks has loaded our add-in and is ready
        /// Now is a good time to create taskpanes, menu bars or anything else
        ///  
        /// NOTE: This call will be made twice, one in the default domain and one in the AppDomain as the SolidDna plug-ins
        /// </summary>
        public static event Action ConnectedToSolidWorks = () => { };

        /// <summary>
        /// Called once SolidWorks has unloaded our add-in
        /// Now is a good time to clean up taskpanes, menu bars or anything else
        /// 
        /// NOTE: This call will be made twice, one in the default domain and one in the AppDomain as the SolidDna plug-ins
        /// </summary>
        public static event Action DisconnectedFromSolidWorks = () => { };

        #endregion

        #region Public Abstract Methods

        /// <summary>
        /// Specific application startup code when SolidWorks is connected 
        /// and before any plug-ins or listeners are informed
        /// 
        /// NOTE: This call will not be in the same AppDomain as the SolidDna plug-ins
        /// </summary>
        /// <returns></returns>
        public abstract void ApplicationStartup();

        /// <summary>
        /// Run immediately when <see cref="ConnectToSW(object, int)"/> is called
        /// to do any pre-setup such as <see cref="PlugInIntegration.UseDetachedAppDomain"/>
        /// </summary>
        public abstract void PreConnectToSolidWorks();

        /// <summary>
        /// Run before loading plug-ins
        /// This call should be used to add plug-ins to be loaded, via <see cref="PlugInIntegration.AddPlugIn{T}"/>
        /// </summary>
        /// <returns></returns>
        public abstract void PreLoadPlugIns();

        /// <summary>
        /// Add any dependency injection items into the DI provider that you would like to use in your application
        /// </summary>
        /// <param name="construction"></param>
        public abstract void ConfigureServices(FrameworkConstruction construction);

        #endregion

        #region SolidWorks Add-in Callbacks

        /// <summary>
        /// Used to pass a callback message onto our plug-ins
        /// </summary>
        /// <param name="arg"></param>
        public void Callback(string arg)
        {
            // Log it
            Logger.LogDebugSource($"SolidWorks Callback fired {arg}");

            PlugInIntegration.OnCallback(arg);
        }
        
        /// <summary>
        /// Called when SolidWorks has loaded our add-in and wants us to do our connection logic
        /// </summary>
        /// <param name="ThisSW">The current SolidWorks instance</param>
        /// <param name="Cookie">The current SolidWorks cookie Id</param>
        /// <returns></returns>
        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            try
            {
                // Get the path to this actual add-in dll
                var assemblyFilePath = this.AssemblyFilePath();
                var assemblyPath = this.AssemblyPath();

                // Setup IoC
                IoC.Setup(assemblyFilePath, construction =>
                    {
                        //  Add SolidDna-specific services
                        // --------------------------------

                        // Add localization manager
                        construction.Services.AddSingleton<ILocalizationManager>(new LocalizationManager
                        {
                            StringResourceDefinition = new ResourceDefinition
                            {
                                Type = ResourceDefinitionType.EmbeddedResource,
                                Location = "AngelSix.SolidDna.Localization.Strings.Strings-{0}.xml",
                                UseDefaultCultureIfNotFound = true,
                            }
                        });

                        //  Configure any services this class wants to add
                        // ------------------------------------------------
                        ConfigureServices(construction);
                    });

                // Log it (critical, so regardless of log level it will write out)
                Logger.LogCriticalSource($"DI Setup complete for {AddInIntegration.SolidWorksAddInTitle}");

                // Log it
                Logger.LogDebugSource($"{SolidWorksAddInTitle} Connected to SolidWorks...");

                // Log it
                Logger.LogDebugSource($"Assembly Path {assemblyFilePath}");

                // Log it
                Logger.LogDebugSource($"Firing PreConnectToSolidWorks...");

                // Fire event
                PreConnectToSolidWorks();

                //
                //   NOTE: Do not need to create it here, as we now create it inside PlugInIntegration.Setup in it's own AppDomain
                //         If we change back to loading directly (not in an app domain) then uncomment this 
                //
                // Store a reference to the current SolidWorks instance
                // Initialize SolidWorks (SolidDNA class)
                //SolidWorks = new SolidWorksApplication((SldWorks)ThisSW, Cookie);

                // Log it
                Logger.LogDebugSource($"Setting AddinCallbackInfo...");

                // Setup callback info
                var ok = ((SldWorks)ThisSW).SetAddinCallbackInfo2(0, this, Cookie);

                // Log it
                Logger.LogDebugSource($"PlugInIntegration Setup...");

                // Setup plug-in application domain
                PlugInIntegration.Setup(assemblyPath, ((SldWorks)ThisSW).RevisionNumber(), Cookie);

                // Log it
                Logger.LogDebugSource($"Firing PreLoadPlugIns...");

                // Any pre-load steps
                PreLoadPlugIns();

                // Log it
                Logger.LogDebugSource($"Configuring PlugIns...");

                // Perform any plug-in configuration
                PlugInIntegration.ConfigurePlugIns(assemblyPath);

                // Log it
                Logger.LogDebugSource($"Firing ApplicationStartup...");

                // Call the application startup function for an entry point to the application
                ApplicationStartup();

                // Log it
                Logger.LogDebugSource($"Firing ConnectedToSolidWorks...");

                // Inform listeners
                ConnectedToSolidWorks();

                // Log it
                Logger.LogDebugSource($"PlugInIntegration ConnectedToSolidWorks...");

                // And plug-in domain listeners
                PlugInIntegration.ConnectedToSolidWorks();

                // Return ok
                return true;
            }
            catch (Exception ex)
            {
                // Try to log it to logger if it made it
                try
                {
                    Logger.LogCriticalSource($"Unexpected error: {ex}");
                }
                catch
                {
                    // Fall-back just write a static log directly
                    File.AppendAllText(Path.ChangeExtension(this.AssemblyFilePath(), "fatal.log.txt"), $"\r\nUnexpected error: {ex}");
                }

                return false;
            }
        }

        /// <summary>
        /// Called when SolidWorks is about to unload our add-in and wants us to do our disconnection logic
        /// </summary>
        /// <returns></returns>
        public bool DisconnectFromSW()
        {
            // Log it
            Logger.LogDebugSource($"{SolidWorksAddInTitle} Disconnected from SolidWorks...");

            // Log it
            Logger.LogDebugSource($"Firing DisconnectedFromSolidWorks...");

            // Inform listeners
            DisconnectedFromSolidWorks();

            // And plug-in domain listeners
            PlugInIntegration.DisconnectedFromSolidWorks();

            // Log it
            Logger.LogDebugSource($"Tearing down...");

            // Clean up plug-in app domain
            PlugInIntegration.Teardown();

            // Dispose SolidWorks COM
            //SolidWorks?.Dispose();
            //SolidWorks = null;

            // Return ok
            return true;
        }

        #endregion

        #region Connected to SolidWorks Event Calls

        /// <summary>
        /// When the add-in has connected to SolidWorks
        /// </summary>
        public static void OnConnectedToSolidWorks()
        {
            // Log it
            Logger.LogDebugSource($"Firing ConnectedToSolidWorks event...");

            ConnectedToSolidWorks();
        }

        /// <summary>
        /// When the add-in has disconnected to SolidWorks
        /// </summary>
        public static void OnDisconnectedFromSolidWorks()
        {
            // Log it
            Logger.LogDebugSource($"Firing DisconnectedFromSolidWorks event...");

            DisconnectedFromSolidWorks();
        }

        #endregion

        #region Com Registration

        /// <summary>
        /// The COM registration call to add our registry entries to the SolidWorks add-in registry
        /// </summary>
        /// <param name="t"></param>
        [ComRegisterFunction()]
        protected static void ComRegister(Type t)
        {
            var keyPath = string.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);

            // Create our registry folder for the add-in
            using (var rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyPath))
            {
                // Load add-in when SolidWorks opens
                rk.SetValue(null, 1);

                //
                // IMPORTANT: 
                //
                //   In this special case, COM register won't load the wrong AngelSix.SolidDna.dll file 
                //   as it isn't loading multiple instances and keeping them in memory
                //            
                //   So loading the path of the AngelSix.SolidDna.dll file that should be in the same
                //   folder as the add-in dll right now will work fine to get the add-in path
                //
                var pluginPath = typeof(PlugInIntegration).CodeBaseNormalized();

                // Let plug-ins configure title and descriptions
                PlugInIntegration.ConfigurePlugIns(pluginPath, log: false);

                // Set SolidWorks add-in title and description
                rk.SetValue("Title", SolidWorksAddInTitle);
                rk.SetValue("Description", SolidWorksAddInDescription);
            }
        }

        /// <summary>
        /// The COM unregister call to remove our custom entries we added in the COM register function
        /// </summary>
        /// <param name="t"></param>
        [ComUnregisterFunction()]
        protected static void ComUnregister(Type t)
        {
            var keyPath = string.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);

            // Remove our registry entry
            Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(keyPath);

        }

        #endregion
    }
}
