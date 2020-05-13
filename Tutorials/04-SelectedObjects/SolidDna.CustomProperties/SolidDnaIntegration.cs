using AngelSix.SolidDna;
using System.IO;

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
            // AppDomainBoundary.UseDetachedAppDomain = true;
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
        public override string AddInDescription => "An example of manipulating Custom Properties inside a SolidWorks model";

        /// <summary>
        /// My Add-in title
        /// </summary>
        public override string AddInTitle => "SolidDNA Custom Properties";

        #endregion

        #region Connect To SolidWorks

        public override void ConnectedToSolidWorks()
        {
            // Create our taskpane
            mTaskpane = new TaskpaneIntegration<TaskpaneUserControlHost>()
            {
                Icon = Path.Combine(this.AssemblyPath(), "logo-small.bmp"),
                WpfControl = new CustomPropertiesUI()
            };

            // Add to taskpane
            mTaskpane.AddToTaskpaneAsync();
        }

        public override void DisconnectedFromSolidWorks()
        {

        }

        #endregion
    }
}
