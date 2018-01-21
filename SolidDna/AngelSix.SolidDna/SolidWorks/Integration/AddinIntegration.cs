using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

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
        /// Now is a good time to create taskpanes, meun bars or anything else
        ///  
        /// NOTE: This call will be made twice, one in the default domain and one in the AppDomain as the SolidDna plug-ins
        /// </summary>
        public static event Action ConnectedToSolidWorks = () => { };

        /// <summary>
        /// Called once SolidWorks has unloaded our add-in
        /// Now is a good time to clean up taskpanes, meun bars or anything else
        /// 
        /// NOTE: This call will be made twice, one in the default domain and one in the AppDomain as the SolidDna plug-ins
        /// </summary>
        public static event Action DisconnectedFromSolidWorks = () => { };

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

        #endregion

        #region SolidWorks Add-in Callbacks

        /// <summary>
        /// Used to pass a callback message onto our plug-ins
        /// </summary>
        /// <param name="arg"></param>
        public void Callback(string arg)
        {
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
            PreConnectToSolidWorks();

            // Setup IoC
            IoCContainer.Ensure();

            //
            //   NOTE: Do not need to create it here, as we now create it inside PlugInIntegration.Setup in it's own AppDomain
            //         If we change back to loading directly (not in an app domain) then uncomment this 
            //
            // Store a reference to the current SolidWorks instance
            // Initialize SolidWorks (SolidDNA class)
            //SolidWorks = new SolidWorksApplication((SldWorks)ThisSW, Cookie);

            // Setup callback info
            var ok = ((SldWorks)ThisSW).SetAddinCallbackInfo2(0, this, Cookie);

            // Setup plug-in application domain
            PlugInIntegration.Setup(((SldWorks)ThisSW).RevisionNumber(), Cookie);

            // Any pre-load steps
            PreLoadPlugIns();

            // Perform any plug-in configuration
            PlugInIntegration.ConfigurePlugIns();

            // Call the application startup function for an entry point to the application
            ApplicationStartup();

            // Inform listeners
            ConnectedToSolidWorks();

            // And plug-in domain listeners
            PlugInIntegration.ConnectedToSolidWorks();

            // Return ok
            return true;
        }

        /// <summary>
        /// Called when SolidWorks is about to unload our add-in and wants us to do our disconnection logic
        /// </summary>
        /// <returns></returns>
        public bool DisconnectFromSW()
        {
            // Inform listeners
            DisconnectedFromSolidWorks();

            // And plug-in domain listeners
            PlugInIntegration.DisconnectedFromSolidWorks();

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
            ConnectedToSolidWorks();
        }

        /// <summary>
        /// When the add-in has disconnected to SolidWorks
        /// </summary>
        public static void OnDisconnectedFromSolidWorks()
        {
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

                // Let plug-ins configure title and descriptions
                PlugInIntegration.ConfigurePlugIns();

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
