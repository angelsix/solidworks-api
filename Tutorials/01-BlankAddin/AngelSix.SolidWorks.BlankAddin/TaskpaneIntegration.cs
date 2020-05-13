using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AngelSix.SolidWorks.BlankAddin
{
    /// <summary>
    /// Our SolidWorks taskpane add-in
    /// </summary>
    public class TaskpaneIntegration : ISwAddin
    {
        #region Private Members

        /// <summary>
        /// The cookie to the current instance of SolidWorks we are running inside of
        /// </summary>
        private int mSwCookie;

        /// <summary>
        /// The taskpane view for our add-in
        /// </summary>
        private TaskpaneView mTaskpaneView;

        /// <summary>
        /// The UI control that is going to be inside the SolidWorks taskpane view
        /// </summary>
        private TaskpaneHostUI mTaskpaneHost;

        /// <summary>
        /// The current instance of the SolidWorks application
        /// </summary>
        private SldWorks mSolidWorksApplication;

        #endregion

        #region Public Members

        /// <summary>
        /// The unique Id to the taskpane used for registration in COM
        /// </summary>
        public const string SWTASKPANE_PROGID = "AngelSix.SolidWorks.BlankAddin.Taskpane";

        #endregion

        #region SolidWorks Add-in Callbacks

        /// <summary>
        /// Called when SolidWorks has loaded our add-in and wants us to do our connection logic
        /// </summary>
        /// <param name="ThisSW">The current SolidWorks instance</param>
        /// <param name="Cookie">The current SolidWorks cookie Id</param>
        /// <returns></returns>
        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            // Store a reference to the current SolidWorks instance
            mSolidWorksApplication = (SldWorks)ThisSW;

            // Store cookie Id
            mSwCookie = Cookie;

            // Setup callback info
            var ok = mSolidWorksApplication.SetAddinCallbackInfo2(0, this, mSwCookie);

            // Create our UI
            LoadUI();

            // Return ok
            return true;
        }

        /// <summary>
        /// Called when SolidWorks is about to unload our add-in and wants us to do our disconnection logic
        /// </summary>
        /// <returns></returns>
        public bool DisconnectFromSW()
        {
            // Clean up our UI
            UnloadUI();

            // Return ok
            return true;
        }

        #endregion

        #region Create UI

        /// <summary>
        /// Create our Taskpane and inject our host UI
        /// </summary>
        private void LoadUI()
        {
            // Find location to our taskpane icon
            var imagePath = Path.Combine(Path.GetDirectoryName(typeof(TaskpaneIntegration).Assembly.CodeBase).Replace(@"file:\", string.Empty), "logo-small.bmp");

            // Create our Taskpane
            mTaskpaneView = mSolidWorksApplication.CreateTaskpaneView2(imagePath, "Woo! My first SwAddin");

            // Load our UI into the taskpane
            mTaskpaneHost = (TaskpaneHostUI)mTaskpaneView.AddControl(TaskpaneIntegration.SWTASKPANE_PROGID, string.Empty);
        }

        /// <summary>
        /// Cleanup the taskpane when we disconnect/unload
        /// </summary>
        private void UnloadUI()
        {
            mTaskpaneHost = null;

            // Remove taskpane view
            mTaskpaneView.DeleteView();

            // Release COM reference and cleanup memory
            Marshal.ReleaseComObject(mTaskpaneView);

            mTaskpaneView = null;
        }

        #endregion

        #region COM Registration

        /// <summary>
        /// The COM registration call to add our registry entries to the SolidWorks add-in registry
        /// </summary>
        /// <param name="t"></param>
        [ComRegisterFunction()]
        private static void ComRegister(Type t)
        {
            var keyPath = string.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);

            // Create our registry folder for the add-in
            using (var rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyPath))
            {
                // Load add-in when SolidWorks opens
                rk.SetValue(null, 1);

                // Set SolidWorks add-in title and description
                rk.SetValue("Title", "My SwAddin");
                rk.SetValue("Description", "All your pixels are belong to us!");
            }
        }

        /// <summary>
        /// The COM unregister call to remove our custom entries we added in the COM register function
        /// </summary>
        /// <param name="t"></param>
        [ComUnregisterFunction()]
        private static void ComUnregister(Type t)
        {
            var keyPath = string.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);

            // Remove our registry entry
            Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(keyPath);

        }

        #endregion
    }
}
