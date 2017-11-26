using AngelSix.SolidDna;
using System.IO;
using System.Threading.Tasks;
using System;

namespace SolidDna.CustomProperties
{
    /// <summary>
    /// Register as a SolidWorks Add-in
    /// </summary>
    public class SolidDnaAddinIntegration : AddInIntegration
    {
        /// <summary>
        /// Specific application start-up code
        /// </summary>
        /// <param name="solidWorks"></param>
        public override void ApplicationStartup()
        {

        }

        public override void PreLoadPlugIns()
        {

        }

        public override void PreConnectToSolidWorks()
        {
            // NOTE: To run in our own AppDomain do the following
            //       Be aware doing so sometimes causes API's to fail
            //       when they try to load dll's
            //
            // PlugInIntegration.UseDetachedAppDomain = true;
        }
    }

    /// <summary>
    /// Register as SolidDna Plugin
    /// </summary>
    public class CustomPropertiesSolidDnaPlugin : SolidPlugIn
    {
        #region Private Members

        /// <summary>
        /// The Taskpane UI for our plug-in
        /// </summary>
        private TaskpaneIntegration<TaskpaneUserControlHost> mTaskpane;

        #endregion

        #region Public Properties

        /// <summary>
        /// My Add-in description
        /// </summary>
        public override string AddInDescription {  get { return "An example of manipulating Custom Properties inside a SolidWorks model"; } }

        /// <summary>
        /// My Add-in title
        /// </summary>
        public override string AddInTitle { get { return "SolidDNA Custom Properties"; } }

        #endregion

        #region Connect To SolidWorks

        public override void ConnectedToSolidWorks()
        {
            // Create our taskpane
            mTaskpane = new TaskpaneIntegration<TaskpaneUserControlHost>()
            {
                Icon = Path.Combine(PlugInIntegration.PlugInFolder, "logo-small.png"),
                WpfControl = new CustomPropertiesUI()
            };

            // Add to taskpane
            mTaskpane.AddToTaskpane();
        }

        public override void DisconnectedFromSolidWorks()
        {

        }

        #endregion
    }
}
