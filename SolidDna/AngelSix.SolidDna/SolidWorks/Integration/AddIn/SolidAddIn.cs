using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Dna;
using Microsoft.Extensions.DependencyInjection;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using static Dna.FrameworkDI;

namespace AngelSix.SolidDna
{
    public abstract class SolidAddIn : ISwAddin
    {
        #region Protected Members

        /// <summary>
        /// Flag if we have loaded into memory (as ConnectedToSolidWorks can happen multiple times if unloaded/reloaded)
        /// </summary>
        protected bool mLoaded;

        #endregion

        #region Public members

        /// <summary>
        /// A list of available plug-ins loaded once SolidWorks has connected
        /// </summary>
        public List<SolidPlugIn> PlugIns = new List<SolidPlugIn>();

        #endregion

        #region Public Properties

        /// <summary>
        /// The title displayed for this SolidWorks Add-in
        /// </summary>
        public string SolidWorksAddInTitle { get; set; } = "AngelSix SolidDna AddIn";

        /// <summary>
        /// The description displayed for this SolidWorks Add-in
        /// </summary>
        public string SolidWorksAddInDescription { get; set; } = "All your pixels are belong to us!";
        
        /// <summary>
        /// If true, loads the plug-ins in their own app-domain
        /// NOTE: Must be set before connecting to SolidWorks
        /// </summary>
        public bool DetachedAppDomain { get; set; }

        #endregion

        #region Public Events

        /// <summary>
        /// Called once SolidWorks has loaded our add-in and is ready.
        /// Now is a good time to create taskpanes, menu bars or anything else.
        ///  
        /// NOTE: This call will be made twice, one in the default domain and one in the AppDomain as the SolidDna plug-ins
        /// </summary>
        public event Action ConnectedToSolidWorks = () => { };

        /// <summary>
        /// Called once SolidWorks has unloaded our add-in.
        /// Now is a good time to clean up taskpanes, menu bars or anything else.
        /// 
        /// NOTE: This call will be made twice, one in the default domain and one in the AppDomain as the SolidDna plug-ins
        /// </summary>
        public event Action DisconnectedFromSolidWorks = () => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SolidAddIn()
        {

        }

        #endregion

        #region Public Abstract / Virtual Methods

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
        /// to do any pre-setup such as <see cref="AppDomainBoundary.UseDetachedAppDomain"/>
        /// </summary>
        public abstract void PreConnectToSolidWorks();

        /// <summary>
        /// Run before loading plug-ins.
        /// This call should be used to add plug-ins to be loaded, via <see cref="PlugInIntegration.AddPlugIn{T}"/>
        /// </summary>
        /// <returns></returns>
        public abstract void PreLoadPlugIns();

        /// <summary>
        /// The method to implement and flag with <see cref="ConfigureServiceAttribute"/>
        /// and a custom name if you want this method to be called during IoC build
        /// </summary>
        /// <param name="construction">The IoC framework construction</param>
        [ConfigureService]
        public virtual void ConfigureServices(FrameworkConstruction construction)
        {
            // Add reference to the add-in integration
            // Which can then be fetched anywhere with
            // IoC.AddIn
            construction.Services.AddSingleton(this);

            // Add localization manager
            Framework.Construction.AddLocalizationManager();
        }

        #endregion

        #region SolidWorks Add-in Callbacks

        /// <summary>
        /// Used to pass a callback message onto our plug-ins
        /// </summary>
        /// <param name="arg"></param>
        public void Callback(string arg)
        {
            // Log it
            Logger?.LogDebugSource($"SolidWorks Callback fired {arg}");

            PlugInIntegration.OnCallback(arg);
        }

        /// <summary>
        /// Called when SolidWorks has loaded our add-in and wants us to do our connection logic
        /// </summary>
        /// <param name="thisSw">The current SolidWorks instance</param>
        /// <param name="cookie">The current SolidWorks cookie Id</param>
        /// <returns></returns>
        public bool ConnectToSW(object thisSw, int cookie)
        {
            try
            {
                // Add this add-in to the list of currently active add-ins.
                AddInIntegration.AddAddIn(this);

                // Fire event
                PreConnectToSolidWorks();

                // Setup application (allowing for AppDomain boundary setup)
                AppDomainBoundary.Setup(this.AssemblyPath(), this.AssemblyFilePath(),
                    // The type of this abstract class will be the class implementing it
                    GetType().Assembly.Location, "");

                // Log it
                Logger?.LogTraceSource($"Fired PreConnectToSolidWorks...");

                // Get the directory path to this actual add-in dll
                var assemblyPath = this.AssemblyPath();

                // Log it
                Logger?.LogDebugSource($"{SolidWorksAddInTitle} Connected to SolidWorks...");

                //
                //   NOTE: Do not need to create it here, as we now create it inside PlugInIntegration.Setup in it's own AppDomain
                //         If we change back to loading directly (not in an app domain) then uncomment this 
                //
                // Store a reference to the current SolidWorks instance
                // Initialize SolidWorks (SolidDNA class)
                //SolidWorks = new SolidWorksApplication((SldWorks)ThisSW, Cookie);

                // Log it
                Logger?.LogDebugSource($"Setting AddinCallbackInfo...");

                // Setup callback info
                var ok = ((SldWorks)thisSw).SetAddinCallbackInfo2(0, this, cookie);

                // Log it
                Logger?.LogDebugSource($"PlugInIntegration Setup...");

                // Setup plug-in application domain
                PlugInIntegration.Setup(assemblyPath, ((SldWorks)thisSw).RevisionNumber(), cookie);

                // Log it
                Logger?.LogDebugSource($"Firing PreLoadPlugIns...");

                // If this is the first load, or we are not loading add-ins 
                // into this domain they need loading every time as they were
                // fully unloaded on disconnect
                if (!mLoaded || AppDomainBoundary.UseDetachedAppDomain)
                {
                    // Any pre-load steps
                    PreLoadPlugIns();

                    // Log it
                    Logger?.LogDebugSource($"Configuring PlugIns...");

                    // Perform any plug-in configuration
                    PlugInIntegration.ConfigurePlugIns(assemblyPath, this);

                    // Now loaded so don't do it again
                    mLoaded = true;
                }

                // Log it
                Logger?.LogDebugSource($"Firing ApplicationStartup...");

                // Call the application startup function for an entry point to the application
                ApplicationStartup();

                // Log it
                Logger?.LogDebugSource($"Firing ConnectedToSolidWorks...");

                // Inform listeners
                ConnectedToSolidWorks();

                // Log it
                Logger?.LogDebugSource($"PlugInIntegration ConnectedToSolidWorks...");

                // And plug-in domain listeners
                PlugInIntegration.ConnectedToSolidWorks(this);

                // Return ok
                return true;
            }
            catch (Exception ex)
            {
                // Log it
                Logger?.LogCriticalSource($"Unexpected error: {ex}");

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
            Logger?.LogDebugSource($"{SolidWorksAddInTitle} Disconnected from SolidWorks...");

            // Log it
            Logger?.LogDebugSource($"Firing DisconnectedFromSolidWorks...");

            // Inform listeners
            DisconnectedFromSolidWorks();

            // And plug-in domain listeners
            PlugInIntegration.DisconnectedFromSolidWorks(this);

            // Log it
            Logger?.LogDebugSource($"Tearing down...");

            // Clean up plug-in app domain
            PlugInIntegration.Teardown();

            // Unload our domain
            AppDomainBoundary.Unload();

            // Remove it from the list and tear down SOLIDWORKS when it was the last add-in.
            AddInIntegration.RemoveAddInAndTearDownSolidWorksWhenLast(this);

            // Reset mLoaded so we can restart this add-in
            mLoaded = false;

            // Return ok
            return true;
        }

        #endregion

        #region Connected to SolidWorks Event Calls

        /// <summary>
        /// When the add-in has connected to SolidWorks
        /// </summary>
        public void OnConnectedToSolidWorks()
        {
            // Log it
            Logger?.LogDebugSource($"Firing ConnectedToSolidWorks event...");

            ConnectedToSolidWorks();
        }

        /// <summary>
        /// When the add-in has disconnected to SolidWorks
        /// </summary>
        public void OnDisconnectedFromSolidWorks()
        {
            // Log it
            Logger?.LogDebugSource($"Firing DisconnectedFromSolidWorks event...");

            DisconnectedFromSolidWorks();
        }

        #endregion

        #region Com Registration

        /// <summary>
        /// The COM registration call to add our registry entries to the SolidWorks add-in registry
        /// </summary>
        /// <param name="t"></param>
        [ComRegisterFunction]
        protected static void ComRegister(Type t)
        {
            // Create new instance of ComRegister add-in to setup DI
            var addIn = new ComRegisterAddInIntegration();

            try
            {
                // Get assembly name
                var assemblyName = t.Assembly.Location;

                // Log it
                Logger?.LogInformationSource($"Registering {assemblyName}");

                // Get registry key path
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

                    // Force auto-discovering plug-in during COM registration
                    PlugInIntegration.AutoDiscoverPlugins = true;

                    Logger?.LogInformationSource("Configuring plugins...");

                    // Let plug-ins configure title and descriptions
                    PlugInIntegration.ConfigurePlugIns(pluginPath, addIn);

                    // Set SolidWorks add-in title and description
                    rk.SetValue("Title", addIn.SolidWorksAddInTitle);
                    rk.SetValue("Description", addIn.SolidWorksAddInDescription);

                    Logger?.LogInformationSource($"COM Registration successful. '{addIn.SolidWorksAddInTitle}' : '{addIn.SolidWorksAddInDescription}'");
                }
            }
            catch (Exception ex)
            {
                Logger?.LogCriticalSource($"COM Registration error. {ex}");
                throw;
            }
        }

        /// <summary>
        /// The COM unregister call to remove our custom entries we added in the COM register function
        /// </summary>
        /// <param name="t"></param>
        [ComUnregisterFunction]
        protected static void ComUnregister(Type t)
        {
            // Get registry key path
            var keyPath = string.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);

            // Remove our registry entry
            Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(keyPath);

        }

        #endregion
    }
}
